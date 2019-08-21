using System;
using System.Configuration;
using System.Data;
using System.Collections.Generic;
using Oracle.ManagedDataAccess.Client;

public class CompassAccessService
{
    private static OracleConnection connection;

    public static bool OpenDatabaseConnection()
    {
        //if (connection != null && connection.State == System.Data.ConnectionState.Open)
        //{
        try
        {
            //CompassADOTestConnection
            //CompassADOLiveConnection
            var connectionStrings = ConfigurationManager.ConnectionStrings["CompassADOLiveConnection"];
            connection = new OracleConnection(connectionStrings.ConnectionString);
            connection.Open();
        }
        catch (Exception ex)
        {
            throw new Exception("Error creating connection to Compass", ex);
        }
        //}
        return true;
    }

    public static void CloseDatabaseConnection()
    {
        if (connection != null && connection.State == System.Data.ConnectionState.Open)
        {
            try
            {
                connection.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating connection to Compass", ex);
            }
        }
    }
    public static GeneralData GetGeneralData(DateTime startDate, DateTime endDate)
    {
        var generalData = new GeneralData();

        if (connection.State == ConnectionState.Open)
        {
            var query = new OracleCommand();
            query.Connection = connection;
            query.CommandType = CommandType.StoredProcedure;

            query.CommandText = "afas.dcipher_general";
            query.Parameters.Add("PI_BEGIN_DT", OracleDbType.Varchar2, ParameterDirection.Input).Value = startDate.Day + "/" + startDate.Month + "/" + startDate.Year; ;
            query.Parameters.Add("PI_END_DT", OracleDbType.Varchar2, ParameterDirection.Input).Value = endDate.Day + "/" + endDate.Month + "/" + endDate.Year;
            OracleParameter inflationParameter = new OracleParameter("cur_indices", OracleDbType.RefCursor, ParameterDirection.Output);
            query.Parameters.Add(inflationParameter);
            OracleParameter exchangeParameter = new OracleParameter("cur_exchange", OracleDbType.RefCursor, ParameterDirection.Output);
            query.Parameters.Add(exchangeParameter);
            OracleParameter pricesParameter = new OracleParameter("cur_prices", OracleDbType.RefCursor, ParameterDirection.Output);
            query.Parameters.Add(pricesParameter);
            OracleParameter assetParameter = new OracleParameter("cur_asset_class", OracleDbType.RefCursor, ParameterDirection.Output);
            query.Parameters.Add(assetParameter);
            OracleParameter fundFactSheetParameter = new OracleParameter("cur_fund_fact_sheet", OracleDbType.RefCursor, ParameterDirection.Output);
            query.Parameters.Add(fundFactSheetParameter);

            try
            {
                OracleDataAdapter dataAdapter = new OracleDataAdapter(query);
                DataSet dataSet = new DataSet();

                dataAdapter.Fill(dataSet);
                foreach (DataTable table in dataSet.Tables)
                {
                    if (table != null)
                    {
                        //Populate Object With Data;
                        if (table.Columns.Contains("OBJECTNAME") && table.Rows.Count >= 1)
                        {
                            string tableName = table.Rows[0]["OBJECTNAME"].ToString();
                            if (!string.IsNullOrEmpty(tableName))
                            {
                                foreach (DataRow row in table.Rows)
                                {
                                    switch (tableName)
                                    {
                                        case "Indices":
                                            generalData.indexPrices.Add(new IndexPrice(row));
                                            break;
                                        case "Exchange":
                                            generalData.exchangeRates.Add(new ExchangeRate(row));
                                            break;
                                        case "UNIT PRICES":
                                            generalData.unitPrices.Add(new UnitPrice(row));
                                            break;
                                        case "Asset Classes":
                                            generalData.assetClasses.Add(new AssetClass(row));
                                            break;
                                        case "Fund Fact Sheet":
                                            generalData.fundFactSheets.Add(new FundFactSheet(row));
                                            break;
                                        default:
                                            break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (OracleException oracleException)
            {
                CloseDatabaseConnection();
                throw oracleException;
            }
            catch (Exception generalException)
            {
                throw generalException;
            }
        }
        return generalData;
    }

    public static List<UnitPrice> GetUnitPricing()
    {
        var unitPrices = new List<UnitPrice>();

        var connectionStrings = ConfigurationManager.ConnectionStrings["CompassADOLiveConnection"];
        var connection = new OracleConnection(connectionStrings.ConnectionString);
        connection.Open();

        if (connection.State == ConnectionState.Open)
        {
            OracleCommand query = new OracleCommand();
            query.Connection = connection;
            query.CommandType = CommandType.StoredProcedure;

            query.CommandText = "AFAS.Get_DCipher_Init_Prices";//"afas.get_dcipher_init_prices";
            OracleParameter unitPricesParameter = new OracleParameter("cur_prices", OracleDbType.RefCursor, ParameterDirection.Output);
            query.Parameters.Add(unitPricesParameter);

            try
            {
                unitPrices = GetData(unitPrices, query, "UNIT PRICES");
            }
            catch (OracleException oracleException)
            {
                CloseDatabaseConnection();
                throw oracleException;
            }
            catch (Exception)
            {
                return unitPrices;
            }
        }
        return unitPrices;
    }

    public class GeneralData
    {
        public List<UnitPrice> unitPrices { get; set; } = new List<UnitPrice>();

        public List<IndexPrice> indexPrices { get; set; } = new List<IndexPrice>();

        public List<ExchangeRate> exchangeRates { get; set; } = new List<ExchangeRate>();

        public List<FundFactSheet> fundFactSheets { get; set; } = new List<FundFactSheet>();

        public List<AssetClass> assetClasses { get; set; } = new List<AssetClass>();
    }

    private static List<T> GetData<T>(List<T> values, OracleCommand query, string objectName)
    {
        OracleDataAdapter da = new OracleDataAdapter(query);
        DataSet ds = new DataSet();

        da.Fill(ds);

        foreach (DataTable table in ds.Tables)
        {
            if (table != null)
            {
                if (table.Rows.Count >= 1)
                {
                    if (table.Rows[0]["OBJECTNAME"].ToString() == objectName)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            values.Add((T)Activator.CreateInstance(typeof(T), row));
                        }
                    }
                }
            }
        }
        return values;
    }
    public class IndexPrice
    {
        public IndexPrice()
        { }

        public IndexPrice(DataRow row)
        {
            foreach (DataColumn column in row.Table.Columns)
            {
                switch (column.ColumnName.ToUpper())
                {
                    case ("OBJECTNAME"):
                        break;
                    case ("IDX_DESCRIPT"):
                        this.IndexID = row[column.ColumnName].ToString();
                        break;
                    case ("EFF_DT"):
                        if (row[column.ColumnName] != DBNull.Value)
                            this.BalanceDate = Convert.ToDateTime(row[column.ColumnName]);
                        break;
                    case ("XPIR_DT"):
                        if (row[column.ColumnName] != DBNull.Value)
                            this.ExpiryDate = Convert.ToDateTime(row[column.ColumnName]);
                        break;
                    case ("IDX_VALUE"):
                        this.Amount = Convert.ToDouble(row[column.ColumnName]);
                        break;
                    default:
                        throw new Exception("Invalid column detected: " + column.ColumnName);

                }

            }
        }

        public string IndexID { get; set; }

        public DateTime BalanceDate { get; set; }

        public DateTime ExpiryDate { get; set; }

        public double Amount { get; set; }
    }

    public class ExchangeRate
    {
        public ExchangeRate()
        { }

        public ExchangeRate(DataRow row)
        {
            foreach (DataColumn column in row.Table.Columns)
            {
                switch (column.ColumnName.ToUpper())
                {
                    case ("OBJECTNAME"):
                        break;
                    case ("EFF_DT"):
                        if (row[column.ColumnName] != DBNull.Value)
                            this.BalanceDate = Convert.ToDateTime(row[column.ColumnName]);
                        break;
                    case ("CROSS_RT"):
                        this.Rate = Convert.ToDouble(row[column.ColumnName]);
                        break;
                    case ("TO_ISO_CURR_CD"):
                        this.CurrencyID = row[column.ColumnName].ToString();
                        break;
                    default:
                        throw new Exception("Invalid column detected: " + column.ColumnName);
                }
            }
        }

        public DateTime BalanceDate { get; set; }

        public double Rate { get; set; }

        public string CurrencyID { get; set; }
    }

    public class UnitPrice
    {
        public UnitPrice()
        { }

        public UnitPrice(DataRow row)
        {
            foreach (DataColumn column in row.Table.Columns)
            {
                switch (column.ColumnName.ToUpper())
                {
                    case ("OBJECTNAME"):
                        break;
                    case ("FD_DESC_ID"):
                        this.FundDescriptionID = row[column.ColumnName].ToString();
                        break;
                    case ("FUND_DESC_ID"):
                        this.FundDescriptionID = row[column.ColumnName].ToString();
                        break;
                    case ("UNIT_VALUE"):
                        this.Amount = Convert.ToDouble(row[column.ColumnName]);
                        break;
                    case ("MONTH_END_DT"):
                        if (row[column.ColumnName] != DBNull.Value)
                            this.MonthEndDate = Convert.ToDateTime(row[column.ColumnName]);
                        break;
                    case ("LAST_MOD_DT"):
                        if (row[column.ColumnName] != DBNull.Value)
                            this.LastModifiedDate = Convert.ToDateTime(row[column.ColumnName]);
                        break;
                    default:
                        throw new Exception("Invalid column detected: " + column.ColumnName);
                }
            }
    }
    

        public string FundDescriptionID { get; set; }

        public double Amount { get; set; }

        public DateTime MonthEndDate { get; set; }

        public DateTime LastModifiedDate { get; set; }

        public override string ToString()
        {
            return $"{FundDescriptionID}|{Amount}|{MonthEndDate}|{LastModifiedDate}";
        }
    }

    public class AssetClass
    {
        public AssetClass()
        { }

        public AssetClass(DataRow assetClassRow)
        {
            foreach (DataColumn column in assetClassRow.Table.Columns)
            {
                try
                {
                    switch (column.ColumnName.ToUpper())
                    {
                        case ("EFF_DT"):
                            this.EffectiveDate = assetClassRow[column.ColumnName].ToString();
                            break;
                        case ("PROD_TYP_CD"):
                            this.ProductTypeCode = assetClassRow[column.ColumnName].ToString();
                            break;
                        case ("PROD_DESC"):
                            this.ProductDescription = assetClassRow[column.ColumnName].ToString();
                            break;
                        case ("FD_DESC_ID"):
                            this.FundDescriptionID = assetClassRow[column.ColumnName].ToString();
                            break;
                        case ("PORTFOLIONAME"):
                            this.PortfolioName = assetClassRow[column.ColumnName].ToString();
                            break;
                        case ("ASSET_CLASS"):
                            this.Class = assetClassRow[column.ColumnName].ToString();
                            break;
                        case ("PCT"):
                            this.Percentage = Convert.ToDouble(assetClassRow[column.ColumnName].ToString());
                            break;
                        case ("PROD_CATEGORY"):
                            this.ProductCategory = assetClassRow[column.ColumnName].ToString();
                            break;
                        case ("FUND_MANAGER"):
                            this.FundManager = assetClassRow[column.ColumnName].ToString();
                            break;
                    }
                }
                catch (InvalidCastException) { }
            }
        }

        public string EffectiveDate { get; set; }

        public string ProductTypeCode { get; set; }

        public string ProductDescription { get; set; }

        public string FundDescriptionID { get; set; }

        public string PortfolioName { get; set; }

        public string Class { get; set; }

        public double Percentage { get; set; }

        public string FundManager { get; set; }

        public string ProductCategory { get; set; }

        public override string ToString()
        {
            return $"{EffectiveDate}|{ProductTypeCode}|{ProductDescription}|{FundDescriptionID}|{PortfolioName}|{Class}|{Percentage}|{FundManager}|{ProductCategory}";
        }
    }

    public class FundFactSheet
    {
        public FundFactSheet()
        { }

        public FundFactSheet(DataRow row)
        {
            foreach (DataColumn column in row.Table.Columns)
            {
                switch (column.ColumnName.ToUpper())
                {
                    case ("OBJECTNAME"):
                        break;
                    case ("FD_DESC_ID"):
                        this.FundDescriptionID = row[column.ColumnName].ToString();
                        break;
                    case ("LONG_FD_NM"):
                        this.Name = row[column.ColumnName].ToString();
                        break;
                    case ("UTILITIES.LONG_TO_VARCHAR2('ALIAS',NTS.REF_KEY)"):
                        this.Location = row[column.ColumnName].ToString();
                        break;
                    default:
                        throw new Exception("Invalid column detected: " + column.ColumnName);
                }
            }
        }

        public string FundDescriptionID { get; set; }

        public string Name { get; set; }

        public string Location { get; set; }
    }
}
