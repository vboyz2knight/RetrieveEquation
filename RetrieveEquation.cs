using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Linq;

namespace RetrieveEquation
{
    public class RetrieveEquation
    {
        public static bool RetrieveMyEquation(string equationID, string myEquationsFilePath, out string theEquation)
        {
            bool bSuccess = true;

            theEquation = string.Empty;

            if (File.Exists(myEquationsFilePath))
            {
                XDocument root = XDocument.Load(myEquationsFilePath);

                var expression = from exp in root.Descendants("resp")
                                 where exp.Element("abbreviation").Value == equationID
                                 select new
                                 {
                                     Command = exp.Element("abbreviation"),
                                     Descriptions = exp.Element("description"),
                                     ID = exp.Element("id"),
                                     Equations = (from l in exp.Descendants("equation")
                                                  select new
                                                  {
                                                      example = l.Value

                                                  })
                                 };

                if (expression.Count() > 0)
                {
                    foreach (var p in expression)
                    {
                        theEquation = p.Equations.FirstOrDefault().example;
                    }
                }
                else
                {
                    bSuccess = false;
                    theEquation = "Error! Unable to locate the require equation.";
                }

            }
            else
            {
                bSuccess = false;
                theEquation = "Error! Equation file does not exist.";
            }

            return bSuccess;
        }
    }
}
