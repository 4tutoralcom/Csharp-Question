// ==============================
// AUTHOR          : Andrew Dittmann
// CREATE DATE     : Jan 31, 2014
// PURPOSE         : The QuestionHandaler Class is created to Store data related to a question the user is set to anwser and keeps Rules for validation.
// SPECIAL NOTES   : Supported Known Data Types String, Char, Numerical(int,double,float...etc)
// ===============================
// Change History:
// Jan 31, 2014     : Created Base Class
// Feb 1, 2014      : Removed getFunctions() repalce with {get; set;}
// Feb 1, 2014      : ctreated a virtural function to write and read user input.
// Feb 2, 2014      : ctreated a virtural function to display the question.
// March 5, 2014      : move for Question Class to Question Handaler class
// March 5, 2014      : replaced new anwserType() default(anwserType) to support String
// March 5, 2014      : replaced new getAnwser method with askQuestion method because to get the anwser you must ask the question.
// March 5, 2014      : replaced range with m_Rules for data validation.
// ===============================
// Planns For future Versions:
// Jan 31, 2014     : Support Vectors, and custom data types by using a virtual functions to allow the user to validate User Created Data Types.
//                  : 
//==================================
using System;
using System.Collections.Generic;

using System.Linq;

using System.Text;
using Question.Rules;

namespace Question.Handaler
{
    public class QuestionHandaler<anwserType>
    {
        private String m_Question;
        private Rule<anwserType>[] m_Rules;    //Array to store pairs of values to check if the Anwser is between an unaceptial value. NOTE:: Should not be set or checked if the value is a string
        private String m_errorMessage;         //Error if the anwser is not with in range and/or is a Error Value
        private Action<QuestionHandaler<anwserType>> display;
        private String m_PreviousAnwser;
        private anwserType m_Anwser;          //Stores the Previous Anwser after a sucessful atempt.
        private Boolean m_IsValid;           //Error if the anwser is not with in range and/or is a Error Value
        private Boolean m_IgnoreCase;          //Ignore case for string an Charcter Questions
        private IOHandler m_InputOutputHandaler;
#region Methods
        public anwserType Anwser
        {
            get
            {
                return this.m_Anwser;
            }
        }
        public String Question
        {
            get
            {
                return this.m_Question;
            }
            set
            {
                this.m_Question=value;
            }
        }
        public String ErrorMessage
        {
            get
            {
                return this.m_errorMessage;
            }
            set
            {
                this.m_errorMessage = value;
            }
        }
        public String PreviousAnwser { 
            get { 
                return m_PreviousAnwser; 
            } 
        }  //Stores the Previous Anwser after a filed atempt to check if it is valid. 
        public IOHandler InputOutputHandaler
        {
            get
            {
                return this.m_InputOutputHandaler;
            }
            set
            {
                this.m_InputOutputHandaler = value;
            }
        }
#endregion
        public Boolean isValidAnwser(string anwser)
        {
            this.testAnwser(anwser);
            return this.m_IsValid;
        }

        public QuestionHandaler(string question = "", string errorMessage = "", Rule<anwserType>[] rules = null, Boolean ignoreCase = false, IOHandler inputOutputHandaler = null, Action<QuestionHandaler<anwserType>> displayFunction = null)
        {
            
            this.Question = question;
            this.ErrorMessage = errorMessage;

            if (rules==null)
            {
                this.m_Rules = new Rule<anwserType>[1];
                this.m_Rules[1] = new Rule<anwserType>();
            }
            this.InputOutputHandaler=(inputOutputHandaler==null)?IOHandler.ConsoleHandler:inputOutputHandaler;
            this.display = (displayFunction == null) ? f_display_default : displayFunction;
            this.m_IgnoreCase = (ignoreCase == null) ? false : ignoreCase;
            this.m_IsValid = false;
        }

        public void testAnwser(string posiableAnwser)
        {
            
            Boolean result = false;
            int rangeResult=0, rangeExpectedResult = 0;
            anwserType anwser = default(anwserType); 
            try
            {
                anwser = (anwserType)Convert.ChangeType(posiableAnwser, typeof(anwserType));
                result = true;
                for(int v =0; v<=1; v++)
                {
                    if (m_ranges[v] != null)
                    {
                        if (typeof(anwserType) == typeof(String) || typeof(anwserType) == typeof(char))
                        {
                            for (int i = 0; i < m_ranges[v].Count(); i++)
                            {
                                if (v == 1 && i == 0)
                                {
                                    result = !result;
                                }
                                if (m_IgnoreCase)
                                {
                                    result = (v == 0 ? (result && m_ranges[v][i].ToString().ToUpper() != anwser.ToString().ToUpper()) : (result || m_ranges[v][i].ToString().ToUpper() == anwser.ToString().ToUpper()));
                                }
                                else
                                {
                                    result = (v == 0 ? (result && m_ranges[v][i].ToString() != anwser.ToString()) : (result || m_ranges[v][i].ToString() == anwser.ToString()));
                                }
                            }
                        }
                        else
                        {
                            if (m_ranges[v].Count() % 2 == 0)
                            {

                                for (int i = 0; i < m_ranges[v].Count();i+=2 )
                                {
                                    if (v == 1 && i == 0)
                                    {
                                        result = !result;
                                    }
                                    rangeExpectedResult = Comparer<anwserType>.Default.Compare(m_ranges[v][i], m_ranges[v][i + 1]);
                                    if (v == 0)
                                    {
                                        result = result && Comparer<anwserType>.Default.Compare(anwser, m_ranges[v][i]) == Comparer<anwserType>.Default.Compare(anwser, m_ranges[v][i + 1]);
                                    }
                                    else
                                    {
                                        result = result && Comparer<anwserType>.Default.Compare(anwser, m_ranges[v][i]) !=  Comparer<anwserType>.Default.Compare(anwser, m_ranges[v][i + 1]) ;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (FormatException e)
            {
                result = false;
                //todo get message for debuging
            }
            catch (OverflowException e)
            {
                result = false;
                //todo get message for debuging
            }
            this.m_Anwser = anwser;
            this.m_IsValid = result;
        }
        public anwserType askQuestion(string posiableAnwser)
        {
            anwserType anwser = default(anwserType);
            if (this.isValidAnwser(posiableAnwser))
            {
                anwser = this.m_Anwser;
            }
            return anwser;
        }
        private void f_display_default(QuestionHandaler<anwserType> question)
        {
            string anwser = "";
            Console.WriteLine(this.Question);
            anwser = Console.ReadLine();
            do
            {
                if (!this.isValidAnwser(anwser))
                {
                    Console.WriteLine(this.ErrorMessage + "\n" + this.Question);
                    anwser = Console.ReadLine();
                }

            } while (!this.isValidAnwser(anwser));

        }//default dispaly function for QuestionHandaler Class, defaults to Question anwserConsole App
    }

}