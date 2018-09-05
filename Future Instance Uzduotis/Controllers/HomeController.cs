using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Future_Instance_Uzduotis.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction("Assignment");
        }

        public class AssignmentResult
        {
            public int MaximumNumber { get; set; }
            public List<List<NumberWithIndexes>> NumbersInLine { get; set; }
            public List<List<int>> gottenVariables { get; set; }
        }

        public class NumberWithIndexes
        {
            public int Number { get; set; }
            public int NumberIndex { get; set; }
        }


        public ActionResult Assignment()
        {
            ViewBag.Title = "Rezultatas";
            List<List<int>> pyramidNumbers = new List<List<int>>
            {
                new List<int> { 215 },
                new List<int> { 192,124 },
                new List<int> { 117,269,442 },
                new List<int> { 218,836,347,235 },
                 new List<int> { 320, 805, 522, 417, 345 },
                 new List<int> {229, 601, 728, 835, 133, 124   },
                 new List<int> {248 ,202, 277, 433, 207, 263, 257   },
                 new List<int> {359, 464, 504, 528, 516, 716, 871, 182   },
                 new List<int> { 461, 441, 426, 656, 863, 560, 380, 171, 923  },
                 new List<int> { 381, 348, 573, 533, 448, 632, 387, 176, 975, 449  },
                 new List<int> { 223, 711, 445, 645, 245, 543, 931, 532, 937, 541 ,444  },
                 new List<int> { 330, 131, 333, 928, 376, 733, 017, 778, 839, 168, 197 ,197  },
                 new List<int> {131, 171, 522, 137, 217, 224, 291, 413, 528 ,520, 227 ,229, 928  },
                 new List<int> {223, 626, 034, 683, 839, 052, 627, 310, 713, 999 ,629 ,817 ,410, 121 },
                 new List<int> { 924, 622, 911, 233, 325, 139, 721, 218, 253, 223, 107 ,233, 230, 124, 233 }
            };

            bool checkedNumbersResult = CheckPyramidFormation(pyramidNumbers);
            AssignmentResult finalResult = new AssignmentResult { MaximumNumber = 0, NumbersInLine = new List<List<NumberWithIndexes>>(), gottenVariables = pyramidNumbers };
            if (checkedNumbersResult)
            {
                finalResult.MaximumNumber = pyramidNumbers.First().First();
                finalResult.NumbersInLine.Add(new List<NumberWithIndexes>
                {
                    new NumberWithIndexes{
                    Number = pyramidNumbers.First().First(),
                NumberIndex = 0
                    }});

                AssignmentResult notBranchingResults = new AssignmentResult
                {
                    MaximumNumber = pyramidNumbers.First().First(),
                    NumbersInLine = new List<List<NumberWithIndexes>>()
                };

                foreach (var pyramidNumberList in pyramidNumbers.Skip(1))
                {
                    List<List<NumberWithIndexes>> tempNumberList = new List<List<NumberWithIndexes>>();
                    List<List<NumberWithIndexes>> listsForRemoving = new List<List<NumberWithIndexes>>();
                    foreach (var lineVarList in finalResult.NumbersInLine)
                    {
                        NumberWithIndexes lastNumber = lineVarList.Last();
                        bool addedNewList = false;
                        if ((lastNumber.Number % 2 == 0 && pyramidNumberList.Skip(lastNumber.NumberIndex).First() % 2 != 0)
                            || (lastNumber.Number % 2 != 0 && pyramidNumberList.Skip(lastNumber.NumberIndex).First() % 2 == 0)
                            )
                        {
                            List<NumberWithIndexes> tempVarlist = lineVarList.ToList();
                            tempVarlist.Add(new NumberWithIndexes { Number = pyramidNumberList.Skip(lastNumber.NumberIndex).First(), NumberIndex = lastNumber.NumberIndex });
                            tempNumberList.Add(tempVarlist);
                            addedNewList = true;
                        }
                        if ((lastNumber.Number % 2 == 0 && pyramidNumberList.Skip(lastNumber.NumberIndex + 1).First() % 2 != 0)
                            || (lastNumber.Number % 2 != 0 && pyramidNumberList.Skip(lastNumber.NumberIndex + 1).First() % 2 == 0)
                            )
                        {
                            List<NumberWithIndexes> tempVarlist2 = lineVarList.ToList();
                            tempVarlist2.Add(new NumberWithIndexes { Number = pyramidNumberList.Skip(lastNumber.NumberIndex + 1).First(), NumberIndex = lastNumber.NumberIndex + 1 });
                            tempNumberList.Add(tempVarlist2);
                            addedNewList = true;
                        }

                        if (!addedNewList)
                        {
                            List<NumberWithIndexes> tempVarlistNonBranching = lineVarList;
                            tempVarlistNonBranching.Add(new NumberWithIndexes { Number = pyramidNumberList.Skip(lastNumber.NumberIndex).First(), NumberIndex = lastNumber.NumberIndex + 1 });
                            if (tempVarlistNonBranching.Select(m => m.Number).Sum() > notBranchingResults.MaximumNumber)
                            {
                                notBranchingResults.MaximumNumber = tempVarlistNonBranching.Select(m => m.Number).Sum();
                                notBranchingResults.NumbersInLine = new List<List<NumberWithIndexes>>
                                    {
                                        tempVarlistNonBranching
                                    };
                            }
                            else if (tempVarlistNonBranching.Select(m => m.Number).Sum() == notBranchingResults.MaximumNumber)
                            {
                                notBranchingResults.NumbersInLine.Add(tempVarlistNonBranching);
                            }
                            listsForRemoving.Add(tempVarlistNonBranching);
                        }
                    }
                    if (listsForRemoving.Any())
                    {
                        foreach (var badList in listsForRemoving)
                        {
                            finalResult.NumbersInLine.Remove(badList);
                        }
                    }
                    if (tempNumberList.Any())
                    {
                        finalResult.NumbersInLine = tempNumberList;
                        finalResult.MaximumNumber = tempNumberList.Max(m => m.Select(n => n.Number).Sum());

                    }
                    if (finalResult.MaximumNumber > notBranchingResults.MaximumNumber)
                    {
                        notBranchingResults.MaximumNumber = finalResult.MaximumNumber;
                        notBranchingResults.NumbersInLine = new List<List<NumberWithIndexes>>();
                    }
                }
                var maxSumAfterBranching = 0;
                try
                {
                    maxSumAfterBranching = finalResult.NumbersInLine.Max(m => m.Select(n => n.Number).Sum());
                }
                catch
                {

                }
                finalResult.NumbersInLine = finalResult.NumbersInLine.Where(m => m.Select(n => n.Number).Sum() == maxSumAfterBranching).ToList();
            }
            else
            {
                ViewBag.Klaida = "Bad variables";
            }
            return View(finalResult);
        }

        private bool CheckPyramidFormation(List<List<int>> numbers)
        {
            bool varCheckResult = true;
            int firstNumberListCount = numbers.First().Count();
            if (firstNumberListCount == 0)
            {
                varCheckResult = !varCheckResult;
            }
            else if (numbers.Count() > 1)
            {
                foreach (var number in numbers.Skip(1))
                {
                    if (number.Count() - 1 == firstNumberListCount)
                    {
                        firstNumberListCount = number.Count();
                    }
                    else
                    {
                        varCheckResult = !varCheckResult;
                        break;
                    }
                }
            }
            return varCheckResult;
        }
    }
}
