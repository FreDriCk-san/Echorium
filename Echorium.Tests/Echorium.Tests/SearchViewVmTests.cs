using Echorium.Models.TableItemM;
using Echorium.ViewModels;
using Echorium.ViewModels.TableItemVM;

namespace Echorium.Tests
{
    public class SearchViewVmTests
    {
        private SearchViewVM _searchViewVM;


        [SetUp]
        public void Setup()
        {
            _searchViewVM = new SearchViewVM();
        }


        [Test]
        public void CreatingVmIsSuccess()
        {
            Assert.That(_searchViewVM, Is.Not.Null);
        }


        [Test]
        public void CreatingDummyModelsIsSuccess()
        {
            for (int i = 0; i < 4; ++i)
            {
                FolderInfoM folderInfoM = new(new System.IO.DirectoryInfo(System.IO.Directory.GetCurrentDirectory()));
                FolderInfoVM folderInfoVM = new(folderInfoM);

                for (int j = 0; j < 5; ++j)
                {
                    FileInfoM fileInfoM = new(folderInfoM.DirectoryDescription.GetFiles()[0]);
                    FileInfoVM fileInfoVM = new(folderInfoVM, fileInfoM);

                    for (int k = 0; k < 6; ++k)
                    {
                        WordInfoM wordInfoM = new(Guid.NewGuid().ToString(), k);
                        WordInfoVM wordInfoVM = new(fileInfoVM, wordInfoM);

                        fileInfoVM.TryAddChild(wordInfoVM);
                    }

                    folderInfoVM.TryAddChild(fileInfoVM);
                }

                _searchViewVM.FolderInfos.Add(folderInfoVM);
            }

            Console.WriteLine($"FolderInfos count: {_searchViewVM.FolderInfos.Count}");
            Assert.That(_searchViewVM.FolderInfos, Has.Count.EqualTo(4));
        }


        [Test]
        public void RegularExpressionWorksCorrect()
        {
            string regularExpression = "[0-5]";
            string nonValidText = "6866866876768687678687";
            string validText = "01020102101201201201";

            var regex = new Regex(regularExpression, RegexOptions.IgnoreCase);
            bool isNotMatch = regex.IsMatch(nonValidText);
            bool isMatch = regex.IsMatch(validText);

            Assert.That(isNotMatch, false);
            Assert.That(isMatch, true);
        }
    }
}