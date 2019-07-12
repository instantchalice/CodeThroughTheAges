using Edge.Config.Model.Utilities;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace UnitTest.Edge.Config.Model
{
    [TestFixture]
    public class EnumExtensionsTests
    {
        [Test]
        public void GetDescription__When_enum_is_doesnt_have_a_description__return_the_enum_as_a_string()
        {
            //Arrange


            //Act
            var description = DurationUnit.Days.GetDescription();

            //Assert
            Assert.IsTrue(description == DurationUnit.Days.Name());
        }

        [Test]
        public void GetDescription__When_enum_has_a_description__return_the_description()
        {
            //Arrange


            //Act
            var description = Ones.Eight.GetDescription();

            //Assert
            Assert.IsTrue(description == "eight");
            Assert.IsFalse(description == Ones.Eight.Name());

        }

        [Test]
        public void ToInt_Given_an_enum_value__returns_the_int_value_of_the_enum()
        {
            //Arrange


            //Act
            var value = Ones.Eight.ToInt();

            //Assert
            Assert.IsTrue(value == 8);
        }

        [Test]
        public void Name_Given_an_enum_value__returns_the_string_value_of_the_enum()
        {
            //Arrange

            //Act
            var description = Ones.Eight.Name();

            //Assert
            Assert.IsTrue(description == Ones.Eight.ToString());
        }
    }

    [TestFixture]
    public class EnumUtilitiesTests
    {
        [Test]
        public void GetEnumAsDictionary__returns_a_dictionary_of_intValue_NameValue()
        {
            //Arrange


            //Act
            var enumDictionary = EnumUtilities<Ones>.GetEnumAsDictionary();

            //Assert
            var count = EnumUtilities<Ones>.GetCount();
            Assert.IsTrue(enumDictionary.Count == count, $"count should be {count}, but is {enumDictionary.Count}");

            Assert.IsTrue(enumDictionary.ContainsKey(8), "dictionary should contain key value of 8");

            var eight = enumDictionary.FirstOrDefault(x => x.Key == 8);
            Assert.IsNotNull(eight);
            Assert.IsTrue(eight.Value == Ones.Eight.Name(), "value of key 8 should be eight");
        }

        [Test]
        public void GetEnumAsSortedList__returns_a_sortedList_of_nameValue_intValue()
        {
            //Arrange


            //Act
            var list = EnumUtilities<Ones>.GetEnumAsSortedList();

            //Assert
            var count = EnumUtilities<Ones>.GetCount();
            Assert.IsTrue(list.Count == count, $"count should be {count}, but is {list.Count}");

            Assert.IsTrue(list.ContainsKey("Eight"), "dictionary should contain key value of 'Eight'");

            var eight = list.FirstOrDefault(x => x.Key == "Eight");
            Assert.IsNotNull(eight);
            Assert.IsTrue(eight.Value == 8, "value of key 'Eight' should be 8");

        }

        [Test]
        public void GetEnumAsOrderedList__returns_a_List_sorted_by_name_value()
        {
            //Arrange


            //Act
            var list = EnumUtilities<Ones>.GetEnumAsOrderedList();

            //Assert
            var count = EnumUtilities<Ones>.GetCount();
            Assert.IsTrue(list.Count == count, $"count should be {count}, but is {list.Count}");

            Assert.IsTrue(list.Contains("Eight"), "list should contain entry 'Eight'");

            var eight = list.FindIndex(x => x == "Eight");
            Assert.IsTrue(eight == 0, "index of value 'Eight' should be 0");
        }

        [Test]
        public void GetEnumDescriptionsAsDictionary__returns_a_dictionary_of_intValue_descriptionOrNameValue()
        {
            //Arrange


            //Act
            var enumDictionary = EnumUtilities<Ones>.GetEnumDescriptionsAsDictionary();

            //Assert
            var count = EnumUtilities<Ones>.GetCount();
            Assert.IsTrue(enumDictionary.Count == count, $"count should be {count}, but is {enumDictionary.Count}");

            Assert.IsTrue(enumDictionary.ContainsKey(8), "dictionary should contain key value of 8");

            var eight = enumDictionary.FirstOrDefault(x => x.Key == 8);
            Assert.IsNotNull(eight);
            Assert.IsTrue(eight.Value == Ones.Eight.GetDescription(), $"value of key 8 should be 'eight', but instead is {eight.Value}");
        }

        [Test]
        public void GetEnumDescriptionsAsList__returns_a_List_of_description_or_name_values()
        {
            //Arrange


            //Act
            var list = EnumUtilities<Ones>.GetEnumDescriptionsAsList();

            //Assert
            var count = EnumUtilities<Ones>.GetCount();
            Assert.IsTrue(list.Count == count, $"count should be {count}, but is {list.Count}");

            Assert.IsTrue(list.Contains("eight"), "list should contain entry 'eight'");
            Assert.IsFalse(list.Contains("Eight"), "list should not contain entry 'Eight'");
        }

        [Test]
        public void FromDescription__returns_the_enum_with_the_matching_description_attribute()
        {
            //Arrange


            //Act
            var eight = EnumUtilities<Ones>.FromDescription("eight");

            //Assert
            Assert.IsTrue(eight == Ones.Eight, "enum should be Eight");
        }

        [Test]
        public void FromDescription__returns_the_enum_default_Value_when_no_enum_exists_with_the_matching_description_attribute()
        {
            //Arrange

            //Act
            var eight = EnumUtilities<Ones>.FromDescription("eighty");

            //Assert
            Assert.IsTrue(eight == Ones.Zero, "enum should be Zero");

        }

        [Test]
        public void FromDescription__returns_the_provided_fallbackValue_when_no_enum_exists_with_the_matching_description_attribute()
        {
            //Arrange

            //Act
            var eight = EnumUtilities<Ones>.FromDescription("eighty", Ones.One);

            //Assert
            Assert.IsTrue(eight == Ones.One, "enum should be One");
        }

        [Test]
        public void ToDescription__if_the_enum_has_a_description_attribute__description_value_is_returned()
        {
            //Arrange

            //Act
            var eight = EnumUtilities<Ones>.ToDescription("Eight");

            //Assert
            Assert.IsTrue(eight == "eight", "description should be 'eight'");

        }

        [Test]
        public void ToDescription__if_the_enum_does_not_have_a_description_attribute__name_value_is_returned()
        {
            //Arrange

            //Act
            var description = EnumUtilities<DurationUnit>.ToDescription("Days");

            //Assert
            Assert.IsTrue(description == "Days", "description should be 'Days'");
        }

        [Test]
        public void ToDescription__if_the_name_does_parse_to_an_enum__empty_string_is_returned()
        {
            //Arrange

            //Act
            var description = EnumUtilities<DurationUnit>.ToDescription("Years");

            //Assert
            Assert.IsTrue(description == string.Empty, "description should be ''");
        }

        [Test]
        public void FromIntValue__When_a_valid_int_is_passed_in__the_corresponding_enum_value_is_returned()
        {
            //Arrange


            //Act
            var val = EnumUtilities<Greek>.FromIntValue(2);

            //Assert
            Assert.IsTrue(val == Greek.Gamma);
        }

        [Test]
        public void FromIntValue__When_an_invalid_int_is_passed_in__the_default_enum_value_is_returned()
        {
            //Arrange

            //Act
            var val = EnumUtilities<Greek>.FromIntValue(99);

            //Assert
            Assert.IsTrue(val == default(Greek));
        }

        [Test]
        public void FromName__When_a_valid_string_is_passed_in__the_corresponding_enum_value_is_returned()
        {
            //Arrange


            //Act
            var val = EnumUtilities<Greek>.FromName("Gamma");

            //Assert
            Assert.IsTrue(val == Greek.Gamma);
        }

        [Test]
        public void FromName__When_an_invalid_string_is_passed_in__the_default_enum_value_is_returned()
        {
            //Arrange


            //Act
            var val = EnumUtilities<Greek>.FromName("Cat");

            //Assert
            Assert.IsTrue(val == default);

        }

        [Test]
        public void ParseObjectToEnum__When_object_can_be_parsed__an_array_of_enums_are_returned()
        {
            //Arrange
            var values = "alpha, beta, zeta, omicron";

            //Act
            var enums = EnumUtilities<Greek>.ParseObjectToEnum(typeof(Greek), values);

            //Assert
            Assert.IsTrue(enums.Contains(Greek.Alpha));
            Assert.IsTrue(enums.Contains(Greek.Beta));
            Assert.IsTrue(enums.Contains(Greek.Zeta));
            Assert.IsTrue(enums.Contains(Greek.Omicron));
        }

        [Test]
        public void FromUnknown__When_description_is_found_first__enum_matching_description_is_returned()
        {
            //Arrange
            var values = new[] { "apple", "beta", "Gamma"};

            //Act
            var unknown = EnumUtilities<Greek>.FromUnknown(Greek.Zeta, values);

            //Assert
            Assert.IsTrue(unknown == Greek.Beta);
        }

        [Test]
        public void FromUnknown__When_name_is_found_first__enum_matching_name_is_returned()
        {
            //Arrange
            var values = new[] { "apple", "Beta", "gamma" };

            //Act
            var unknown = EnumUtilities<Greek>.FromUnknown(Greek.Zeta, values);

            //Assert
            Assert.IsTrue(unknown == Greek.Beta);
        }

        [Test]
        public void FromUnknown__When_no_match_is_found__fallback_value_is_returned()
        {
            //Arrange
            var values = new[] { "apple", "berry", "Cherry" };

            //Act
            var unknown = EnumUtilities<Greek>.FromUnknown(Greek.Zeta, values);

            //Assert
            Assert.IsTrue(unknown == Greek.Zeta);
        }
    }

    [TestFixture]
    public class StringExtensionsTests
    {
        [Test]
        public void CleanForSqlTest()
        {
            //Arrange
            var withQuotes = "A 'quoted' string";

            //Act
            var withEscapedQuotes = withQuotes.CleanForSql();

            //Assert
            Assert.IsTrue(withEscapedQuotes == "A ''quoted'' string");
        }

        [Test]
        public void CleanTest()
        {
            //Arrange
            var withCharacters = @"A /quoted\, "" string";

            //Act
            var clean = withCharacters.Clean();

            //Assert
            Assert.IsTrue(clean == "A quoted  string");
        }

        [Test]
        public void QuoteTest()
        {
            //Arrange
            var withNoQuotes = "A quoted string";

            //Act
            var quoted = withNoQuotes.Quote();

            //Assert
            Assert.IsTrue(quoted == "'A quoted string'");
        }

        [Test]
        public void QuoteDoubleTest()
        {
            //Arrange
            var withNoQuotes = "A quoted string";

            //Act
            var quoted = withNoQuotes.QuoteDouble();

            //Assert
            Assert.IsTrue(quoted == @"""A quoted string""");

        }

        [Test]
        public void WithSlashTest()
        {
            //Arrange
            var slashed = @"\aPath";

            //Act
            var updated = slashed.WithSlash();

            //Assert
            Assert.IsTrue(updated == @"aPath\");
        }

        [Test]
        public void WithSlashEmptyTest()
        {
            //Arrange
            var slashed = @"";

            //Act
            var updated = slashed.WithSlash();

            //Assert
            Assert.IsTrue(updated == "");
        }

        [Test]
        public void ToIntTest()
        {
            //Arrange
            var instString = @"1";

            //Act
            var updated = instString.ToInt();

            //Assert
            Assert.IsTrue(updated == 1);
        }

        [Test]
        public void ToIntNotIntTest()
        {
            //Arrange
            var instString = @"one";

            //Act
            var updated = instString.ToInt();

            //Assert
            Assert.IsTrue(updated == -1);
        }

        [Test]
        public void ToBoolTrueTest()
        {
            //Arrange
            var boolString = @"True";
            var lowerBoolString = @"true";


            //Act
            var updated = boolString.ToBool();
            var lowerUpdated = lowerBoolString.ToBool();

            //Assert
            Assert.IsTrue(updated);
            Assert.IsTrue(lowerUpdated);
        }

        [Test]
        public void ToBoolFalseTest()
        {
            //Arrange
            var boolString = @"False";
            var lowerBoolString = @"false";


            //Act
            var updated = boolString.ToBool();
            var lowerUpdated = lowerBoolString.ToBool();

            //Assert
            Assert.IsFalse(updated);
            Assert.IsFalse(lowerUpdated);
        }

        [Test]
        public void ToBool1Test()
        {
            //Arrange
            var boolString = @"1";

            //Act
            var updated = boolString.ToBool();

            //Assert
            Assert.IsTrue(updated);
        }

        [Test]
        public void ToBool0Test()
        {
            //Arrange
            var boolString = @"0";


            //Act
            var updated = boolString.ToBool();

            //Assert
            Assert.IsFalse(updated);
        }

        [Test]
        public void ToBoolInvalidTest()
        {
            //Arrange
            var boolString = @"cat";

            //Act
            var updated = boolString.ToBool();

            //Assert
            Assert.IsFalse(updated);
        }

        [Test]
        public void RemoveWhiteSpacesTest()
        {
            //Arrange
            var input = @"a l p h a";

            //Act
            var updated = input.RemoveWhiteSpaces();

            //Assert
            Assert.IsTrue(updated == "alpha");
        }

        [Test]
        public void ReplaceWhiteSpacesTest()
        {
            //Arrange
            var input = @"a l p h a";

            //Act
            var updated = input.RemoveWhiteSpaces(".");

            //Assert
            Assert.IsTrue(updated == "a.l.p.h.a");
        }

        [Test]
        public void TrimNullSafeTest()
        {
            //Arrange
            var input = @" a l p h a ";

            //Act
            var updated = input.TrimNullSafe();

            //Assert
            Assert.IsTrue(updated == "a l p h a");
        }

        [Test]
        public void TrimNullSafeNullTest()
        {
            //Arrange
            string input = null;

            //Act
            var updated = input.TrimNullSafe();

            //Assert
            Assert.IsTrue(updated == null);
        }

        [Test]
        public void TruncateTest()
        {
            //Arrange
            var input = @"0123456789";

            //Act
            var updated = input.Truncate(5);

            //Assert
            Assert.IsTrue(updated == "01234");
        }

        [Test]
        public void TruncateShortTest()
        {
            //Arrange
            var input = @"0123456789";

            //Act
            var updated = input.Truncate(15);

            //Assert
            Assert.IsTrue(updated == "0123456789");
        }

        [Test]
        public void TruncateWithPaddingTruncatedTest()
        {
            //Arrange
            var input = @"0123456789";

            //Act
            var updated = input.TruncateWithPadding(5, ',',true);

            //Assert
            Assert.IsTrue(updated == "01234");
        }

        [Test]
        public void TruncateWithPaddingPaddedFrontTest()
        {
            //Arrange
            var input = @"0123456789";

            //Act
            var updated = input.TruncateWithPadding(15, '0', false);

            //Assert
            Assert.IsTrue(updated == "000000123456789");
        }

        [Test]
        public void TruncateWithPaddingPaddedEndTest()
        {
            //Arrange
            var input = @"0123456789";

            //Act
            var updated = input.TruncateWithPadding(15, '0', true);

            //Assert
            Assert.IsTrue(updated == "012345678900000");
        }

        [Test]
        public void IsEmptyWithWhitespaceTest()
        {
            //Arrange
            var input = @"     ";

            //Act
            var updated = input.IsEmpty();

            //Assert
            Assert.IsTrue(updated);
        }

        [Test]
        public void IsEmptyWithNoWhitespaceTest()
        {
            //Arrange
            var input = @"";

            //Act
            var updated = input.IsEmpty();

            //Assert
            Assert.IsTrue(updated);
        }

        [Test]
        public void IsEmptyWithNoInfoTest()
        {
            //Arrange
            var input = @"abc";

            //Act
            var updated = input.IsEmpty();

            //Assert
            Assert.IsFalse(updated);
        }

        [Test]
        public void AppendNullableLineValidLineTest()
        {
            //Arrange
            var sb = new StringBuilder();
            sb.AppendLine("LineOne");

            //Act
            sb.AppendNullableLine("LineTwo");

            //Assert
            Assert.IsTrue(sb.Length == 18);
        }

        [Test]
        public void AppendNullableLineNullTest()
        {
            //Arrange
            var sb = new StringBuilder();
            sb.AppendLine("LineOne");

            //Act
            sb.AppendNullableLine(null);

            //Assert
            Assert.IsTrue(sb.Length == 9);
        }

        [Test]
        public void AppendNullableLineEmptyStringTest()
        {
            //Arrange
            var sb = new StringBuilder();
            sb.AppendLine("LineOne");

            //Act
            sb.AppendNullableLine(string.Empty);

            //Assert
            Assert.IsTrue(sb.Length == 9);
        }

        [Test]
        public void IsIntegerIsIntegerTest()
        {
            //Arrange
            var value = "1";

            //Act
            var isInteger = value.IsInteger();

            //Assert
            Assert.IsTrue(isInteger);
        }

        [Test]
        public void IsIntegerIsNotIntegerTest()
        {
            //Arrange
            var value = "one";

            //Act
            var isInteger = value.IsInteger();

            //Assert
            Assert.IsFalse(isInteger);
        }

        [Test]
        public void TokenizerTest()
        {
            //Arrange
            var values = "alpha, BETA- -Gamma, omiCron";
            var delimeters = new char[] {',', '-', ' '};
            
            //Act
            var list = values.Tokenizer(delimeters);

            //Assert
            Assert.IsTrue(list.Count == 4);
            Assert.IsTrue(list.Contains("alpha"));
            Assert.IsTrue(list.Contains("BETA"));
            Assert.IsTrue(list.Contains("Gamma"));
            Assert.IsTrue(list.Contains("omiCron"));
            Assert.IsFalse(list.Contains("beta"));
        }

        [Test]
        public void Tokenizer2Test()
        {
            //Arrange
            var values = "alpha, BETA- -Gamma, omiCron";
            var delimeters = new char[] { ',', '-', ' ' };

            //Act
            var list = values.Tokenizer(delimeters, true);

            //Assert
            Assert.IsTrue(list.Count == 4);
            Assert.IsTrue(list.Contains("ALPHA"));
            Assert.IsTrue(list.Contains("BETA"));
            Assert.IsTrue(list.Contains("GAMMA"));
            Assert.IsTrue(list.Contains("OMICRON"));
            Assert.IsFalse(list.Contains("omiCron"));
        }

        [Test]
        public void ContainsNormalTest()
        {
            //Arrange
            var input = @"abcdefghijklmno";
            var substring = "def";

            //Act
            var hasSubstring = input.Contains(substring);

            //Assert
            Assert.IsTrue(hasSubstring);
        }

        [Test]
        public void ContainsWrongCasingTest()
        {
            //Arrange
            var input = @"abcdefghijklmno";
            var substring = "DEF";

            //Act
            var hasSubstring = input.Contains(substring);

            //Assert
            Assert.IsFalse(hasSubstring);
        }

        [Test]
        public void ContainsIgnoreCaseTest()
        {
            //Arrange
            var input = @"abcdefghijklmno";
            var substring = "DEF";

            //Act
            var hasSubstring = input.Contains(substring, StringComparison.InvariantCultureIgnoreCase);

            //Assert
            Assert.IsTrue(hasSubstring);
        }

        [Test]
        public void ContainsTest()
        {
            //Arrange
            var input = @"abcdefghijklmno";
            var substring = "DEF";

            //Act
            var hasSubstring = input.Contains(substring, StringComparison.Ordinal);

            //Assert
            Assert.IsFalse(hasSubstring);
        }

        [Test]
        public void SubstringUpTo_StringExists_Test()
        {
            //Arrange
            var input = @"abcdefghijklmno";

            //Act
            var substring = input.SubstringUpTo("g");

            //Assert
            Assert.IsTrue(substring == "abcdef");
        }

        [Test]
        public void SubstringUpTo_StringDoesNotExist_Test()
        {
            //Arrange
            var input = @"abcdefghijklmno";

            //Act
            var substring = input.SubstringUpTo("p");

            //Assert
            Assert.IsTrue(substring == string.Empty);
        }

        [Test]
        public void SubstringUpTo_SearchStringEmpty_Test()
        {
            //Arrange
            var input = @"abcdefghijklmno";

            //Act
            var substring = input.SubstringUpTo("");

            //Assert
            Assert.IsTrue(substring == "abcdefghijklmno");
        }

        [Test]
        public void SubstringUpTo_SearchStringNull_Test()
        {
            //Arrange
            var input = @"abcdefghijklmno";

            //Act
            var substring = input.SubstringUpTo(null);

            //Assert
            Assert.IsTrue(substring == "abcdefghijklmno");
        }

        [Test]
        public void SubstringAfter_StringExists_Test()
        {
            //Arrange
            var input = @"abcdefghijklmno";

            //Act
            var substring = input.SubstringAfter("g");

            //Assert
            Assert.IsTrue(substring == "hijklmno");
        }

        [Test]
        public void SubstringAfter_StringDoesNotExist_Test()
        {
            //Arrange
            var input = @"abcdefghijklmno";

            //Act
            var substring = input.SubstringAfter("p");

            //Assert
            Assert.IsTrue(substring == string.Empty);
        }

        [Test]
        public void SubstringAfter_SearchStringEmpty_Test()
        {
            //Arrange
            var input = @"abcdefghijklmno";

            //Act
            var substring = input.SubstringAfter("");

            //Assert
            Assert.IsTrue(substring == "abcdefghijklmno");
        }

        [Test]
        public void SubstringAfter_SearchStringNull_Test()
        {
            //Arrange
            var input = @"abcdefghijklmno";

            //Act
            var substring = input.SubstringAfter(null);

            //Assert
            Assert.IsTrue(substring == "abcdefghijklmno");
        }

        [Test]
        public void SubstringAfterLast_StringExists_Test()
        {
            //Arrange
            var input = @"abcdeafghijkalmno";

            //Act
            var substring = input.SubstringAfterLast("a");

            //Assert
            Assert.IsTrue(substring == "lmno");
        }

        [Test]
        public void SubstringAfterLast_StringDoesNotExist_Test()
        {
            //Arrange
            var input = @"abcdeafghijkalmno";

            //Act
            var substring = input.SubstringAfterLast("p");

            //Assert
            Assert.IsTrue(substring == string.Empty);
        }

        [Test]
        public void SubstringAfterLast_SearchStringEmpty_Test()
        {
            //Arrange
            var input = @"abcdeafghijkalmno";

            //Act
            var substring = input.SubstringAfterLast("");

            //Assert
            Assert.IsTrue(substring == "abcdeafghijkalmno");
        }

        [Test]
        public void SubstringAfterLast_SearchStringNull_Test()
        {
            //Arrange
            var input = @"abcdeafghijkalmno";

            //Act
            var substring = input.SubstringAfterLast(null);

            //Assert
            Assert.IsTrue(substring == "abcdeafghijkalmno");
        }

        [Test]
        public void SubstringBetweenTest()
        {
            //string SubstringBetween(this string value, string firstString, string secondString)
            //returns the substring of the value after the first string to before the secondstring
            //if either searchstring is not found, returns string.empty
            //if the searchStrings are out of order (second before the first) returns empty


            //Arrange
            var input = @"abcdefghijklmno";
            var first = "d";
            var second = "k";

            //Act
            var validSubstring = input.SubstringBetween(first, second);
            var badFirstSubstring = input.SubstringBetween("p", second);
            var badSecondSubstring = input.SubstringBetween(first, "p");
            var emptyFirstSubstring = input.SubstringBetween(string.Empty, second);
            var emptySecondSubstring = input.SubstringBetween(first, string.Empty);
            var nullFirstSubstring = input.SubstringBetween(null, second);
            var nullSecondSubstring = input.SubstringBetween(first, null);
            var outOfOrderSubstring = input.SubstringBetween(second, first);

            //Assert
            Assert.IsTrue(validSubstring == "efghij");
            Assert.IsTrue(badFirstSubstring == string.Empty);
            Assert.IsTrue(badSecondSubstring == string.Empty);
            Assert.IsTrue(emptyFirstSubstring == string.Empty);
            Assert.IsTrue(emptySecondSubstring == string.Empty);
            Assert.IsTrue(nullFirstSubstring == string.Empty);
            Assert.IsTrue(nullSecondSubstring == string.Empty);
            Assert.IsTrue(outOfOrderSubstring == string.Empty);
        }

        [Test]
        public void EqualsCiTest()
        {
            //Arrange
            var first = "abcdefg";
            var second = "AbCdEfG";

            //Act
            var areEqual = first.EqualsCi(second);

            //Assert
            Assert.IsTrue(areEqual);
        }
        
    }

    [TestFixture]
    public class NumberExtensionsTests
    {
        [Test]
        public void LongToOrdinalTest()
        {
            //Arrange
            long input1 = 1;
            long input2 = 2;
            long input3 = 3;
            long input4 = 4;
            long input11 = 11;
            long input21 = 21;

            //Act
            var output1 =  input1.ToOrdinal();
            var output2 =  input2.ToOrdinal();
            var output3 =  input3.ToOrdinal();
            var output4 =  input4.ToOrdinal();
            var output11 = input11.ToOrdinal();
            var output21 = input21.ToOrdinal();

            //Assert
            Assert.IsTrue(output1 == "1st");
            Assert.IsTrue(output2 == "2nd");
            Assert.IsTrue(output3 == "3rd");
            Assert.IsTrue(output4 == "4th");
            Assert.IsTrue(output11 == "11th");
            Assert.IsTrue(output21 == "21st");
        }

        [Test]
        public void IntToOrdinalTest()
        {
            //Arrange
            int input1 = 1;
            int input2 = 2;
            int input3 = 3;
            int input4 = 4;
            int input11 = 11;
            int input21 = 21;

            //Act
            var output1 = input1.ToOrdinal();
            var output2 = input2.ToOrdinal();
            var output3 = input3.ToOrdinal();
            var output4 = input4.ToOrdinal();
            var output11 = input11.ToOrdinal();
            var output21 = input21.ToOrdinal();

            //Assert
            Assert.IsTrue(output1 == "1st");
            Assert.IsTrue(output2 == "2nd");
            Assert.IsTrue(output3 == "3rd");
            Assert.IsTrue(output4 == "4th");
            Assert.IsTrue(output11 == "11th");
            Assert.IsTrue(output21 == "21st");

        }

        [Test]
        public void ToWordsTest()
        {
            //Arrange
            var input1 = 1;
            var input12 = 12;
            var input33 = 33;
            var input104 = 104;
            var input395 = 395;
            var input1006 = 1006;
            var input32487 = 32487;

            //Act
            var output1 = input1.ToWords();
            var output12 = input12.ToWords();
            var output33 = input33.ToWords();
            var output104 = input104.ToWords();
            var output395 = input395.ToWords();
            var output1006 = input1006.ToWords();
            var output32487 = input32487.ToWords();

            //Assert
            Assert.IsTrue(output1 == "one");
            Assert.IsTrue(output12 == "twelve");
            Assert.IsTrue(output33 == "thirty-three");
            Assert.IsTrue(output104 == "one hundred four");
            Assert.IsTrue(output395 == "three hundred ninety-five");
            Assert.IsTrue(output1006 == "one thousand six");
            Assert.IsTrue(output32487 == "thirty-two thousand four hundred eighty-seven");
        }

        [Test]
        public void RadiansToDegreesTest()
        {
            //Arrange
            var radians = 1.5708;
            
            //Act
            var degrees = radians.RadiansToDegrees();

            //Assert
            Assert.IsTrue(Math.Floor(degrees) == 90);
        }

        [Test]
        public void RadiansToDegreesNullableTest()
        {
            //Arrange
            double? radians = null;

            //Act
            var degrees = radians.RadiansToDegrees();

            //Assert
            Assert.IsNull(degrees);
        }

        [Test]
        public void DoubleAreEqualTest()
        {
            //Arrange
            double value1 = 10.9333;
            double value2 = 10.9334;

            //Act
            var valuesAreEqual0 = value1.AreEqual(value2, 0);
            var valuesAreEqual2 = value1.AreEqual(value2, 2);
            var valuesAreEqual4 = value1.AreEqual(value2, 4);
            var valuesAreEqualn = value1.AreEqual(value2, -1);

            //Assert
            Assert.IsTrue(valuesAreEqual0, "values at precision 0 should be equal but aren't");
            Assert.IsTrue(valuesAreEqual2, "values at precision 2 should be equal but aren't");
            Assert.IsFalse(valuesAreEqual4, "values at precision 4 should not be equal but are");
            Assert.IsFalse(valuesAreEqualn, "values at precision -1 should not be equal but are");
        }

        [Test]
        public void NullableDoubleAreEqualTest()
        {
            //bool AreEqual(this double? firstValue, double? secondValue, int precision = 0)
            //Rounds two double values to a specified number of decimal places and returns equality
            //if precision is less than zero returns false
            //if either value (but not both) are null returns false
            //if both values are null returns true

            //Arrange
            double? value1 = 10.9333;
            double? value2 = 10.9334;
            double? value1Null = null;
            double? value2Null = null;

            //Act
            var valuesAreEqual0 = value1.AreEqual(value2, 0);
            var valuesAreEqual2 = value1.AreEqual(value2, 2);
            var valuesAreEqual4 = value1.AreEqual(value2, 4);
            var valuesAreEqualn = value1.AreEqual(value2, -1);
            var valueOneIsNull = value1Null.AreEqual(value2);
            var valueTwoIsNull = value1.AreEqual(value2Null);
            var bothValuesAreNull = value1Null.AreEqual(value2Null);

            //Assert
            Assert.IsTrue(valuesAreEqual0, "values at precision 0 should be equal but aren't");
            Assert.IsTrue(valuesAreEqual2, "values at precision 2 should be equal but aren't");
            Assert.IsFalse(valuesAreEqual4, "values at precision 4 should not be equal but are");
            Assert.IsFalse(valuesAreEqualn, "values at precision -1 should not be equal but are");
            Assert.IsFalse(valueOneIsNull, "comparison with first value null should not be equal but are");
            Assert.IsFalse(valueTwoIsNull, "comparison with second value null should not be equal but are");
            Assert.IsTrue(bothValuesAreNull, "comparison with both values null should be equal but are not");
        }

        [Test]
        public void SecondDoubleNullableAreEqualTest()
        {
            //Arrange
            double value1 = 10.9333;
            double? value2 = 10.9334;
            double? value2Null = null;

            //Act
            var valuesAreEqual0 = value1.AreEqual(value2, 0);
            var valuesAreEqual2 = value1.AreEqual(value2, 2);
            var valuesAreEqual4 = value1.AreEqual(value2, 4);
            var valuesAreEqualn = value1.AreEqual(value2, -1);
            var valueTwoIsNull = value1.AreEqual(value2Null);

            //Assert
            Assert.IsTrue(valuesAreEqual0, "values at precision 0 should be equal but aren't");
            Assert.IsTrue(valuesAreEqual2, "values at precision 2 should be equal but aren't");
            Assert.IsFalse(valuesAreEqual4, "values at precision 4 should not be equal but are");
            Assert.IsFalse(valuesAreEqualn, "values at precision -1 should not be equal but are");
            Assert.IsFalse(valueTwoIsNull, "comparison with second value null should not be equal but are");
        }

        [Test]
        public void FirstDoubleNullableAreEqualTest()
        {
            //Arrange
            double? value1 = 10.9333;
            double value2 = 10.9334;
            double? value1Null = null;

            //Act
            var valuesAreEqual0 = value1.AreEqual(value2, 0);
            var valuesAreEqual2 = value1.AreEqual(value2, 2);
            var valuesAreEqual4 = value1.AreEqual(value2, 4);
            var valuesAreEqualn = value1.AreEqual(value2, -1);
            var valueOneIsNull = value1Null.AreEqual(value2);

            //Assert
            Assert.IsTrue(valuesAreEqual0, "values at precision 0 should be equal but aren't");
            Assert.IsTrue(valuesAreEqual2, "values at precision 2 should be equal but aren't");
            Assert.IsFalse(valuesAreEqual4, "values at precision 4 should not be equal but are");
            Assert.IsFalse(valuesAreEqualn, "values at precision -1 should not be equal but are");
            Assert.IsFalse(valueOneIsNull, "comparison with first value null should not be equal but are");
        }

        [Test]
        public void IsBetweenBothInclusiveTest()
        {
            //Arrange
            int value5 = 5;
            int value10 = 10;
            int value15 = 15;

            var lowerBoundry = 5;
            var upperBoundry = 15;

            //Act
            var assert10 = value10.IsBetween(lowerBoundry, upperBoundry, true);
            var assert5 = value5.IsBetween(lowerBoundry, upperBoundry, true);
            var assert15 = value15.IsBetween(lowerBoundry, upperBoundry, true);

            var assertNot10 = value10.IsBetween(lowerBoundry, upperBoundry, false);
            var assertNot5 = value5.IsBetween(lowerBoundry, upperBoundry, false);
            var assertNot15 = value15.IsBetween(lowerBoundry, upperBoundry, false);

            //Assert
            Assert.IsTrue(assert10);
            Assert.IsTrue(assert5);
            Assert.IsTrue(assert15);

            Assert.IsTrue(assertNot10);
            Assert.IsFalse(assertNot5);
            Assert.IsFalse(assertNot15);
        }

        [Test]
        public void IsBetweenTestEitherInclusive()
        {
            //Arrange
            int value5 = 5;
            int value15 = 15;

            var lowerBoundry = 5;
            var upperBoundry = 15;

            //Act
            var lowerInclusive5 = value5.IsBetween(lowerBoundry, upperBoundry, true, true);
            var lowerNotInclusive5 = value5.IsBetween(lowerBoundry, upperBoundry, false, true);

            var upperInclusive15 = value15.IsBetween(lowerBoundry, upperBoundry, true, true);
            var upperNotInclusive15 = value15.IsBetween(lowerBoundry, upperBoundry, true, false);

            //Assert
            Assert.IsTrue(lowerInclusive5);
            Assert.IsTrue(upperInclusive15);

            Assert.IsFalse(lowerNotInclusive5);
            Assert.IsFalse(upperNotInclusive15);
        }

        [Test]
        public void ToLesserValueThanTest()
        {
            //Arrange
            int smaller = 5;
            int larger = 15;

            //Act
            var outputSmallerFirst = smaller.ToLesserValueThan(larger);
            var outputLargerFirst = larger.ToLesserValueThan(smaller);

            //Assert
            Assert.IsTrue(outputSmallerFirst == smaller);
            Assert.IsTrue(outputLargerFirst == smaller);
        }

        [Test]
        public void ToGreaterValueThanTest()
        {
            //Arrange
            int smaller = 5;
            int larger = 15;

            //Act
            var outputSmallerFirst = smaller.ToGreaterValueThan(larger);
            var outputLargerFirst = larger.ToGreaterValueThan(smaller);

            //Assert
            Assert.IsTrue(outputSmallerFirst == larger);
            Assert.IsTrue(outputLargerFirst == larger);
        }
    }

    [TestFixture]
    public class DateTimeExtensionsTests
    {
        [Test]
        public void ToNewTest()
        {
            //Arrange
            var date = DateTime.Now;

            //Act
            var newCopy = date.ToNew();

            //Assert
            Assert.AreNotEqual(date, newCopy);
            Assert.AreEqual(date.Day, newCopy.Day);
            Assert.AreEqual(date.Month, newCopy.Month);
            Assert.AreEqual(date.Year, newCopy.Year);
            Assert.AreEqual(date.Hour, newCopy.Hour);
            Assert.AreEqual(date.Minute, newCopy.Minute);
        }

        [Test]
        public void StartOfDayTest()
        {
            //Arrange
            var date = DateTime.Now;

            //Act
            var output = date.StartOfDay();

            //Assert
            Assert.AreNotEqual(date, output);
            Assert.AreEqual(date.Day, output.Day);
            Assert.AreEqual(date.Month, output.Month);
            Assert.AreEqual(date.Year, output.Year);
            Assert.AreEqual(0, output.Hour);
            Assert.AreEqual(0, output.Minute);
            Assert.AreEqual(0, output.Second);
        }

        [Test]
        public void StartOfDayNullableTest()
        {
            //Arrange
            DateTime? date = null;

            //Act
            var output = date.StartOfDay();

            //Assert
            Assert.IsNull(output);
        }

        [Test]
        public void EndOfDayTest()
        {
            //Arrange
            var date = DateTime.Now;

            //Act
            var output = date.EndOfDay();

            //Assert
            Assert.AreNotEqual(date, output);
            Assert.AreEqual(date.Day, output.Day);
            Assert.AreEqual(date.Month, output.Month);
            Assert.AreEqual(date.Year, output.Year);
            Assert.AreEqual(23, output.Hour);
            Assert.AreEqual(59, output.Minute);
            Assert.AreEqual(59, output.Second);
        }

        [Test]
        public void EndOfDayNullableTest()
        {
            //Arrange
            DateTime? date = null;

            //Act
            var output = date.EndOfDay();

            //Assert
            Assert.IsNull(output);
        }

        [Test]
        public void NextDayTest()
        {
            //Arrange
            var date = DateTime.Now;

            //Act
            var output = date.NextDay();

            //Assert
            Assert.AreNotEqual(date, output);
            Assert.AreEqual(date.Day + 1, output.Day);
            Assert.AreEqual(date.Month, output.Month);
            Assert.AreEqual(date.Year, output.Year);
            Assert.AreEqual(0, output.Hour);
            Assert.AreEqual(0, output.Minute);
            Assert.AreEqual(0, output.Second);
        }

        [Test]
        public void NextDayNullableTest()
        {
            //Arrange
            DateTime? date = null;

            //Act
            var output = date.NextDay();

            //Assert
            Assert.IsNull(output);
        }

        [Test]
        public void BetweenTest()
        {
            //Arrange
            DateTime baseDate = new DateTime(2020,11,11,11,11,11);
            DateTime? day11 = new DateTime?(baseDate);
            DateTime? day16 = day11.Value.AddDays(5);
            DateTime? day6 = day11.Value.AddDays(-5);
            DateTime? day1 = day11.Value.AddDays(-10);
            DateTime? day21 = day11.Value.AddDays(10);

            //Act
            var eleventh = day11.Between(day6, day16);
            var first = day1.Between(day6, day16);
            var twentyFirst = day21.Between(day6, day16);
            var sixth = day6.Between(day6, day16);
            var sixteenth = day16.Between(day6, day16);

            //Assert
            Assert.IsTrue(eleventh);
            Assert.IsTrue(sixth);
            Assert.IsTrue(sixteenth);

            Assert.IsFalse(first);
            Assert.IsFalse(twentyFirst);

        }

        [Test]
        public void IsInPastOrNotSetTest()
        {
            //Arrange
            DateTime now = DateTime.Now;
            DateTime future = now.AddYears(10);
            DateTime past = now.AddYears(-10);
            DateTime notSet = DateTime.MinValue;
            
            //Act
            var outputFuture = future.IsInPastOrNotSet();
            var outputPast = past.IsInPastOrNotSet();
            var outputNotSet = notSet.IsInPastOrNotSet();

            //Assert
            Assert.False(outputFuture);
            Assert.True(outputPast);
            Assert.True(outputNotSet);
        }

        [Test]
        public void IsInPastOrNotSetInclusiveTest()
        {
            //Arrange
            DateTime notSet = DateTime.MinValue;

            //Act
            var outputInclusive = DateTime.Today.IsInPastOrNotSet(true);
            var outputExclusive = DateTime.Today.IsInPastOrNotSet(false);

            var outputNotSet = notSet.IsInPastOrNotSet();

            //Assert
            Assert.True(outputInclusive);
            Assert.False(outputExclusive);
            Assert.True(outputNotSet);
        }

        [Test]
        public void IsInFutureOrNotSetTest()
        {
            //Arrange
            DateTime now = DateTime.Now;
            DateTime future = now.AddYears(10);
            DateTime past = now.AddYears(-10);
            DateTime notSet = DateTime.MinValue;

            //Act
            var outputFuture = future.IsInFutureOrNotSet();
            var outputPast = past.IsInFutureOrNotSet();
            var outputNotSet = notSet.IsInFutureOrNotSet();

            //Assert
            Assert.True(outputFuture);
            Assert.False(outputPast);
            Assert.True(outputNotSet);
        }

        [Test]
        public void IsInFutureOrNotSetInclusiveTest()
        {
            //Arrange
            DateTime notSet = DateTime.MinValue;

            //Act
            var outputInclusive = DateTime.Today.IsInFutureOrNotSet(true);
            var outputExclusive = DateTime.Today.IsInFutureOrNotSet(false);

            var outputNotSet = notSet.IsInFutureOrNotSet();

            //Assert
            Assert.True(outputInclusive);
            Assert.False(outputExclusive);
            Assert.True(outputNotSet);
        }

        [Test]
        public void IsInPastTest()
        {
            //Arrange
            DateTime now = DateTime.Now;
            DateTime future = now.AddYears(10);
            DateTime past = now.AddYears(-10);
            DateTime notSet = DateTime.MinValue;

            //Act
            var outputFuture = future.IsInPast();
            var outputPast = past.IsInPast();
            var outputNotSet = notSet.IsInPast();

            //Assert
            Assert.False(outputFuture);
            Assert.True(outputPast);
            Assert.False(outputNotSet);
        }

        [Test]
        public void IsInPastInclusiveTest()
        {
            //Arrange
            DateTime notSet = DateTime.MinValue;

            //Act
            var outputInclusive = DateTime.Today.IsInPast(true);
            var outputExclusive = DateTime.Today.IsInPast(false);

            var outputNotSet = notSet.IsInPast();

            //Assert
            Assert.True(outputInclusive);
            Assert.False(outputExclusive);
            Assert.False(outputNotSet);
        }

        [Test]
        public void IsInFutureTest()
        {
            //Arrange
            DateTime now = DateTime.Now;
            DateTime future = now.AddYears(10);
            DateTime past = now.AddYears(-10);
            DateTime notSet = DateTime.MinValue;

            //Act
            var outputFuture = future.IsInFuture();
            var outputPast = past.IsInFuture();
            var outputNotSet = notSet.IsInFuture();

            //Assert
            Assert.True(outputFuture);
            Assert.False(outputPast);
            Assert.False(outputNotSet);
        }

        [Test]
        public void IsInFutureInclusiveTest()
        {
            //Arrange
            DateTime notSet = DateTime.MinValue;

            //Act
            var outputInclusive = DateTime.Today.IsInFuture(true);
            var outputExclusive = DateTime.Today.IsInFuture(false);

            var outputNotSet = notSet.IsInFuture();

            //Assert
            Assert.True(outputInclusive);
            Assert.False(outputExclusive);
            Assert.False(outputNotSet);
        }

        [Test]
        public void IsNotSetTest()
        {
            //Arrange
            DateTime? nullDate = null;
            DateTime? notSet = DateTime.MinValue;

            //Act
            var outputNull = nullDate.IsNotSet();
            var outputNotSet = notSet.IsNotSet();

            //Assert
            Assert.True(outputNull);
            Assert.True(outputNotSet);
        }

        [Test]
        public void IsNotSetWithDefaultTest()
        {
            //Arrange
            DateTime? nullDate = null;
            DateTime? notSet = DateTime.MinValue;
            DateTime? fallbackValue = DateTime.Now;

            //Act
            var outputNull = nullDate.IsNotSet();
            var outputNotSet = notSet.IsNotSet();
            var outputSetToDefault = fallbackValue.IsNotSet(fallbackValue);
            var outputNotSetToDefault = fallbackValue.IsNotSet(fallbackValue.Value.AddDays(1));

            //Assert
            Assert.True(outputNull);
            Assert.True(outputNotSet);
            Assert.True(outputSetToDefault);
            Assert.False(outputNotSetToDefault);
        }

        [Test]
        public void StartOfWeekTest()
        {
            //Arrange
            DateTime baseDate = new DateTime(2025, 5, 14); //This is a wednesday

            //Act
            var outputStartOfWeek = baseDate.StartOfWeek(DayOfWeek.Monday);

            //Assert
            Assert.True(outputStartOfWeek == baseDate.AddDays(-2));
        }

        [Test]
        public void StartOfQuarterTest()
        {
            //Arrange
            DateTime q1 = new DateTime(2025, 2, 14); 
            DateTime q2 = new DateTime(2025, 5, 14); 
            DateTime q3 = new DateTime(2025, 8, 14); 
            DateTime q4 = new DateTime(2025, 11, 14); 

            //Act
            var outputStartOfQ1 = q1.StartOfQuarter();
            var outputStartOfQ2 = q2.StartOfQuarter();
            var outputStartOfQ3 = q3.StartOfQuarter();
            var outputStartOfQ4 = q4.StartOfQuarter();

            //Assert
            Assert.True(outputStartOfQ1.Month == 1);
            Assert.True(outputStartOfQ1.Day == 1);

            Assert.True(outputStartOfQ2.Month == 4);
            Assert.True(outputStartOfQ2.Day == 1);

            Assert.True(outputStartOfQ3.Month == 7);
            Assert.True(outputStartOfQ3.Day == 1);

            Assert.True(outputStartOfQ4.Month == 10);
            Assert.True(outputStartOfQ4.Day == 1);
        }

        [Test]
        public void EndOfQuarterTest()
        {
            //Arrange
            DateTime q1 = new DateTime(2025, 2, 14);
            DateTime q2 = new DateTime(2025, 5, 14);
            DateTime q3 = new DateTime(2025, 8, 14);
            DateTime q4 = new DateTime(2025, 11, 14);

            //Act
            var outputEndOfQ1 = q1.EndOfQuarter();
            var outputEndOfQ2 = q2.EndOfQuarter();
            var outputEndOfQ3 = q3.EndOfQuarter();
            var outputEndOfQ4 = q4.EndOfQuarter();

            //Assert
            Assert.True(outputEndOfQ1.Month == 3);
            Assert.True(outputEndOfQ1.Day == 31);

            Assert.True(outputEndOfQ2.Month == 6);
            Assert.True(outputEndOfQ2.Day == 30);

            Assert.True(outputEndOfQ3.Month == 9);
            Assert.True(outputEndOfQ3.Day == 30);

            Assert.True(outputEndOfQ4.Month == 12);
            Assert.True(outputEndOfQ4.Day == 31);
        }

    }

    [TestFixture]
    public class ListExtensionsTests
    {
        [Test]
        public void ContainsCiTest()
        {
            //Arrange
            var array = new string[] {"Alpha", "Beta", "Gamma", "Delta", "Epsilon"};
            var containedValue = "beta";
            var invalidValue = "Eta";

            //Act
            var valid = array.ContainsCi(containedValue);
            var invalid = array.ContainsCi(invalidValue);

            //Assert
            Assert.IsTrue(valid);
            Assert.IsFalse(invalid);
        }

        [Test]
        public void EnumerableToObservableCollectionTest()
        {
            //Arrange
            var list = new List<string> {"Alpha", "Beta", "Gamma", "Delta", "Epsilon"};

            //Act
            var enumerable = list.AsEnumerable();
            var output = enumerable.ToObservableCollection();

            //Assert
            Assert.IsTrue(list.Count == output.Count);
        }

        [Test]
        public void ListToStringCollectionTest()
        {
            //Arrange
            var list = new List<string> { "Alpha", "Beta", "Gamma", "Delta", "Epsilon" };

            //Act
            var output = list.ToCollection();

            //Assert
            Assert.IsTrue(list.Count == output.Count);
        }

        [Test]
        public void ToListTest()
        {
            //Arrange
            var list = new StringCollection() { "Alpha", "Beta", "Gamma", "Delta", "Epsilon" };

            //Act
            var output = list.ToList();

            //Assert
            Assert.IsTrue(list.Count == output.Count);
        }

        [Test]
        public void ListToBuiltStringTest()
        {
            //Arrange
            var list = new List<string> { "Alpha", "Beta", "Gamma", "Delta", "Epsilon" };

            //Act
            var output = list.ToBuiltString();

            //Assert
            Assert.IsTrue(output == "Alpha\r\nBeta\r\nGamma\r\nDelta\r\nEpsilon\r\n");
        }

        [Test]
        public void DictionaryToBuiltStringTest()
        {
            //Arrange
            var list = new Dictionary<int, string>();
            list.Add(1, Greek.Alpha.Name());
            list.Add(2, Greek.Beta.Name());
            list.Add(3, Greek.Gamma.Name());
            list.Add(4, Greek.Delta.Name());
            list.Add(5, Greek.Epsilon.Name());

            //Act
            var output = list.ToBuiltString();
            
            //Assert
            Assert.IsTrue(output == "1: Alpha\r\n2: Beta\r\n3: Gamma\r\n4: Delta\r\n5: Epsilon\r\n");
        }

        [Test]
        public void MergeUniqueDictionaryWithDictionaryTest()
        {
            //Arrange
            var currentList = new Dictionary<int, string>();
            currentList.Add(0, Ones.Zero.Name());
            currentList.Add(1, Ones.One.Name());

            var listToMerge = new Dictionary<int, string>();
            listToMerge.Add(0, Ones.Three.Name());
            listToMerge.Add(5, Ones.One.Name());

            //Act
            currentList.MergeUnique(listToMerge);

            //Assert
            Assert.IsTrue(currentList.Count == 3);
            Assert.IsTrue(currentList.ContainsKey(5));
            Assert.IsTrue(currentList.ContainsKey(1));
            Assert.IsTrue(currentList.ContainsKey(0));
            Assert.IsTrue(currentList.Values.Count(x => x == Ones.One.Name()) == 2);
        }

        [Test]
        public void MergeUniqueListWithListTest()
        {
            //Arrange
            var currentList = new List<string>();
            currentList.Add(Ones.Zero.Name());
            currentList.Add(Ones.One.Name());

            var listToMerge = new List<string>();
            listToMerge.Add(Ones.Three.Name());
            listToMerge.Add(Ones.One.Name());

            //Act
            currentList.MergeUnique(listToMerge);

            //Assert
            Assert.IsTrue(currentList.Count == 3);
            Assert.IsTrue(currentList.Contains(Ones.Zero.Name()));
            Assert.IsTrue(currentList.Contains(Ones.One.Name()));
            Assert.IsTrue(currentList.Contains(Ones.Three.Name()));
        }

        [Test]
        public void MergeUniqueListWithListByKeyTest()
        {
            //Arrange
            var currentList = new List<TestClass>();
            currentList.Add(new TestClass(Greek.Alpha.Name(), Greek.Beta.Name(), Greek.Eta.Name()));
            currentList.Add(new TestClass(Greek.Omicron.Name(), Greek.Delta.Name(), Greek.Theta.Name()));

            var listToMerge = new List<TestClass>();
            listToMerge.Add(new TestClass(Greek.Alpha.Name(), Greek.Beta.Name(), Greek.Zeta.Name()));
            listToMerge.Add(new TestClass(Greek.Delta.Name(), Greek.Gamma.Name(), Greek.Eta.Name()));

            //Act
            currentList.MergeUnique(listToMerge, "Subject");

            //Assert
            Assert.IsTrue(currentList.Count == 3);
            Assert.IsTrue(currentList.Any(x => x.Subject == Greek.Beta.Name()));
            Assert.IsTrue(currentList.Any(x => x.Subject == Greek.Delta.Name()));
            Assert.IsTrue(currentList.Any(x => x.Subject == Greek.Gamma.Name()));
        }

        private class TestClass
        {
            public string Title { get; set; }
            public string Subject { get; set; }
            public string Body { get; set; }

            public TestClass(string title, string subject, string body)
            {
                Title = title;
                Subject = subject;
                Body = body;
            }
        }

        [Test]
        public void MergeUniqueObservableCollectionWithListTest()
        {
            //Arrange
            var currentList = new ObservableCollection<string>();
            currentList.Add(Ones.Zero.Name());
            currentList.Add(Ones.One.Name());

            var listToMerge = new List<string>();
            listToMerge.Add(Ones.Three.Name());
            listToMerge.Add(Ones.One.Name());

            //Act
            currentList.MergeUnique(listToMerge);

            //Assert
            Assert.IsTrue(currentList.Count == 3);
            Assert.IsTrue(currentList.Contains(Ones.Zero.Name()));
            Assert.IsTrue(currentList.Contains(Ones.One.Name()));
            Assert.IsTrue(currentList.Contains(Ones.Three.Name()));
        }

        [Test]
        public void MergeUniqueObservableCollectionWithObservableCollectionTest()
        {
            //Arrange
            var currentList = new ObservableCollection<string>();
            currentList.Add(Ones.Zero.Name());
            currentList.Add(Ones.One.Name());

            var listToMerge = new ObservableCollection<string>();
            listToMerge.Add(Ones.Three.Name());
            listToMerge.Add(Ones.One.Name());

            //Act
            currentList.MergeUnique(listToMerge);

            //Assert
            Assert.IsTrue(currentList.Count == 3);
            Assert.IsTrue(currentList.Contains(Ones.Zero.Name()));
            Assert.IsTrue(currentList.Contains(Ones.One.Name()));
            Assert.IsTrue(currentList.Contains(Ones.Three.Name()));
        }

        [Test]
        public void AddUniqueItemToList__AddUnique__Test()
        {
            //Arrange
            var currentList = new List<string>();
            currentList.Add(Ones.Zero.Name());
            currentList.Add(Ones.One.Name());

            //Act
            currentList.AddUnique(Ones.Three.Name());

            //Assert
            Assert.IsTrue(currentList.Count == 3);
            Assert.IsTrue(currentList.Contains(Ones.Zero.Name()));
            Assert.IsTrue(currentList.Contains(Ones.One.Name()));
            Assert.IsTrue(currentList.Contains(Ones.Three.Name()));
        }

        [Test]
        public void AddUniqueItemToList__NotUnique__Test()
        {
            //Arrange
            var currentList = new List<string>();
            currentList.Add(Ones.Zero.Name());
            currentList.Add(Ones.One.Name());

            //Act
            currentList.AddUnique(Ones.One.Name());

            //Assert
            Assert.IsTrue(currentList.Count == 2);
            Assert.IsTrue(currentList.Contains(Ones.Zero.Name()));
            Assert.IsTrue(currentList.Contains(Ones.One.Name()));
        }

        [Test]
        public void AddUniqueItemToDictionary__AddUnique_Test()
        {
            //Arrange
            var currentList = new Dictionary<int, string>();
            currentList.Add(0, Ones.Zero.Name());
            currentList.Add(1, Ones.One.Name());

            //Act
            currentList.AddUnique(5, Ones.One.Name());

            //Assert
            Assert.IsTrue(currentList.Count == 3);
            Assert.IsTrue(currentList.ContainsKey(5));
            Assert.IsTrue(currentList.ContainsKey(1));
            Assert.IsTrue(currentList.ContainsKey(0));
        }


        [Test]
        public void AddUniqueItemToDictionary__NotUniqueTest()
        {
            //Arrange
            var currentList = new Dictionary<int, string>();
            currentList.Add(0, Ones.Zero.Name());
            currentList.Add(1, Ones.One.Name());

            //Act
            currentList.AddUnique(1, Ones.Five.Name());

            //Assert
            Assert.IsTrue(currentList.Count == 2);
            Assert.IsTrue(currentList.ContainsKey(1));
            Assert.IsTrue(currentList.ContainsKey(0));
        }

        [Test]
        public void RemoveOutliersTest()
        {
            //Arrange
            var currentList = new ObservableCollection<string>();
            currentList.Add(Ones.Zero.Name());
            currentList.Add(Ones.One.Name());

            var listToMerge = new List<string>();
            listToMerge.Add(Ones.Three.Name());
            listToMerge.Add(Ones.One.Name());

            //Act
            currentList.RemoveOutliers(listToMerge);

            //Assert
            Assert.IsTrue(currentList.Count == 1);
            Assert.IsTrue(currentList.Contains(Ones.One.Name()));
        }

        [Test]
        public void ToLowerListTest()
        {
            //Arrange
            var array = new[] {Ones.Three.Name(), Ones.One.Name(), Ones.Five.Name()};

            //Act
            var list = array.ToLowerList();

            //Assert
            Assert.IsTrue(list.Count == 3);
            Assert.IsTrue(list.Contains("three"));
            Assert.IsTrue(list.Contains("one"));
            Assert.IsTrue(list.Contains("five"));
            Assert.IsFalse(list.Contains("Five"));
        }

        [Test]
        public void SafeContainsTest()
        {
            //Arrange
            var array = new[] { Ones.Three.Name(), Ones.One.Name(), Ones.Five.Name() };

            //Act
            var containsLower = array.SafeContains("three");
            var containsUpper = array.SafeContains("Three");

            //Assert
            Assert.IsTrue(containsLower);
            Assert.IsTrue(containsUpper);
        }

        [Test]
        public void MaxCountInGroupTest()
        {
            //Arrange
            var currentList = new List<TestClass>();
            currentList.Add(new TestClass(Greek.Alpha.Name(), Greek.Beta.Name(), Greek.Eta.Name()));
            currentList.Add(new TestClass(Greek.Omicron.Name(), Greek.Delta.Name(), Greek.Theta.Name()));
            currentList.Add(new TestClass(Greek.Alpha.Name(), Greek.Beta.Name(), Greek.Zeta.Name()));
            currentList.Add(new TestClass(Greek.Delta.Name(), Greek.Gamma.Name(), Greek.Eta.Name()));

            //Act
            var max = currentList.MaxCountInGroup(x => x.Body);

            //Assert
            Assert.IsTrue(max == 2);
        }

        [Test]
        public void ToNewListTest()
        {
            //Arrange
            var value = Greek.Beta.Name();

            //Act
            var list = value.ToNewList();

            //Assert
            Assert.IsNotNull(list);
            Assert.IsTrue(list.Count == 1);
        }

        [Test]
        public void AddRangeTest()
        {
            //Arrange
            var currentList = new Dictionary<int, string>();
            currentList.Add(0, Ones.Zero.Name());
            currentList.Add(1, Ones.One.Name());

            var listToMerge = new Dictionary<int, string>();
            listToMerge.Add(2, Ones.Two.Name());
            listToMerge.Add(3, Ones.Three.Name());

            //Act
            currentList.AddRange(listToMerge);

            //Assert
            Assert.IsTrue(currentList.Count == 4);
            Assert.IsTrue(currentList.ContainsKey(2));
            Assert.IsTrue(currentList.ContainsKey(1));
            Assert.IsTrue(currentList.ContainsKey(0));
        }
    }

    [TestFixture]
    public class ParseExtensionsTests
    {
        [Test]
        public void ToBoolTest()
        {
            //bool ToBool(this object value, bool fallbackValue)

            //Arrange
            object boolObject = "true";

            //Act
            var boolValue = boolObject.ToBool(false);

            //Assert
            Assert.IsTrue(boolValue);

            //Arrange
            boolObject = "value";

            //Act
            boolValue = boolObject.ToBool(true);

            //Assert
            Assert.IsTrue(boolValue);
        }

        [Test]
        public void ToEnumTest()
        {
            //T ToEnum<T>(this string value, T fallbackValue) where T : struct

            //Arrange
            string enumObject = "Gamma";

            //Act
            var enumValue = enumObject.ToEnum<Greek>(Greek.Alpha);

            //Assert
            Assert.IsTrue(enumValue == Greek.Gamma);

            //Arrange
            enumObject = "unicorn";

            //Act
            enumValue = enumObject.ToEnum<Greek>(Greek.Alpha);

            //Assert
            Assert.IsTrue(enumValue == Greek.Alpha);
        }

        [Test]
        public void ToDoubleTest()
        {
            //double ToDouble(this object value, double fallbackValue)
            //Arrange
            object doubObject = "100";

            //Act
            var doubValue = doubObject.ToDouble(1);

            //Assert
            Assert.IsTrue(doubValue == 100);

            //Arrange
            doubObject = "one hundred";

            //Act
            doubValue = doubObject.ToDouble(1);

            //Assert
            Assert.IsTrue(doubValue == 1);

        }
    }
    
    [TestFixture]
    public class GenericTypeExtensionsTests
    {
        [Test]
        public void IsInArrayTest()
        {
            //Arrange
            var array = new[] { Ones.Three.Name(), Ones.One.Name(), Ones.Five.Name() };

            //Act
            var isIn = Ones.One.Name().IsIn(array);
            var isNotIn = Ones.Ten.Name().IsIn(array);

            //Assert
            Assert.IsTrue(isIn);
            Assert.IsFalse(isNotIn);
        }
    }
}
