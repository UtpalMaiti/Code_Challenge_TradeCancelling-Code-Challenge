using CodeScreen.Assessments.TradeCancelling.src.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Reflection;


namespace CodeScreen.Assessments.TradeCancelling
{
    static class ExcessiveTradeCancellingChecker
    {
       public static List<string> CompaniesInvolvedInExcessiveCancellations()
        {
            // Returns the list of companies that are involved in excessive cancelling.
            //TODO Implement

            try
            {
                var CsvDataSet = GetCsvDataFromFile();

                if (CsvDataSet.Count == 0)
                {
                    new List<string>();
                }

                //Making Dataset for 60s Calculation
                var MakingDatasetFor60s = CsvDataSet
                       .Select(r => new CompanyWiseDate
                       {
                           CompanyName = r.CompanyName,
                           Startdate = r.OrderDate,
                           EndDate = r.OrderDate.AddSeconds(60),
                           order = 0,
                           cancel = 0
                       })
                       .Distinct().ToList();

                int i = MakingDatasetFor60s.Count;

                //Filtering Based on Condition Apply
                MakingDatasetFor60s.ForEach(r =>
                {
                    i = i - 1;
                    r.order = CsvDataSet.Where(x => x.CompanyName == r.CompanyName && x.OrderDate >= r.Startdate && x.OrderDate <= r.EndDate && x.OrderType == "D").Sum(x => x.Quantity);
                    r.cancel = CsvDataSet.Where(x => x.CompanyName == r.CompanyName && x.OrderDate >= r.Startdate && x.OrderDate <= r.EndDate && x.OrderType == "F").Sum(x => x.Quantity);
                });

                var ExcessiveCancellingDs = MakingDatasetFor60s.Where(r => (r.cancel / r.order) > (1 / 3)).ToList();

                var ExcessiveCancellingCompanyNames = ExcessiveCancellingDs.Select(r => r.CompanyName).Distinct().ToList();

                return ExcessiveCancellingCompanyNames == null ? new List<string>() : ExcessiveCancellingCompanyNames;
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message.ToString());
                return new List<string>();
            }
        }

        public static int TotalNumberOfWellBehavedCompanies()
        {
            // Returns the total number of companies that are not involved in any excessive cancelling.
            //TODO Implement

            try
            {
                var CsvDataSet = GetCsvDataFromFile();

                var CsvDataSetCompanyNames = CsvDataSet.Select(r => r.CompanyName).Distinct().ToList();

                var CompaniesInvolvedInExcessiveCancellationsNameList = CompaniesInvolvedInExcessiveCancellations();

                var CompaniesNotInvolvedInExcessiveCancellationsNameList = CsvDataSetCompanyNames.Except(CompaniesInvolvedInExcessiveCancellationsNameList);

                return CompaniesNotInvolvedInExcessiveCancellationsNameList.Count();
            }
            catch (Exception)
            {

                return 0;
            }

        }

        public static List<TradesMessage> GetCsvDataFromFile()
        {
            List<TradesMessage> TradesMessages = new List<TradesMessage>();

            using (var reader = new StreamReader(@"..\..\..\Trades.data"))
            {

                while (!reader.EndOfStream)
                {
                    try
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(',');
                        var tradesMessage = new TradesMessage()
                        { CompanyName = values[1], OrderDate = Convert.ToDateTime(values[0]), OrderType = values[2], Quantity = Convert.ToInt32(values[3]) };

                        TradesMessages.Add(tradesMessage);
                    }
                    catch (Exception)
                    {

                        continue;
                    }

                }
            }

            return TradesMessages;
        }


    }
}
