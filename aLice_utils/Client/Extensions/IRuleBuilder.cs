using System.Text.RegularExpressions;
using aLice_utils.Shared.Models;
using CatSdk.Utils;
using FluentValidation;

namespace aLice_utils.Client.Extensions;

public static class IRuleBuilder
{
    public static IRuleBuilderOptions<T, string> IsUnderXByte<T>(this IRuleBuilder<T, string> ruleBuilder, int num)
    {
        return ruleBuilder
            .Must((str) => Converter.Utf8ToBytes(str).Length <= num)
            .WithMessage("メッセージは1023byte以下である必要があります。");
    }
    
    public static IRuleBuilderOptions<T, string> IsAlphabetic<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .Must((str) => Regex.IsMatch(str, "^[A-z]+$"))
                .WithMessage("アルファベット以外の文字が含まれています。");
    }
    
    public static IRuleBuilderOptions<T, string> IsNodeUrl<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .Must((str) => Regex.IsMatch(str, @"^https:\/\/.*:3001$"))
            .WithMessage("正しいNodeのURLを入力してください。（ssl）");
    }
    
    public static IRuleBuilderOptions<T, string> IsOver<T>(this IRuleBuilder<T, string> ruleBuilder, int num)
    {
        return ruleBuilder
            .Must((str) =>
            {
                try
                {
                    var t = int.Parse(str);
                    return t >= num;
                } catch {
                    return false;
                }
            })
            .WithMessage($"{num}以上でなければいけません");
    }
    
    public static IRuleBuilderOptions<T, string> IsUnder<T>(this IRuleBuilder<T, string> ruleBuilder, int num)
    {
        return ruleBuilder
            .Must((str) =>
            {
                try
                {
                    var t = int.Parse(str);
                    return t <= num;
                } catch {
                    return false;
                }
            })
            .WithMessage($"{num}以下でなければいけません");
    }
    
    public static IRuleBuilderOptions<T, string> IsXCharacters<T>(this IRuleBuilder<T, string> ruleBuilder, int num)
    {
        return ruleBuilder
            .Must(id => id.Length == num)
            .WithMessage($"{num}文字でなければいけません");
    }
    public static IRuleBuilderOptions<T, string> IsHex<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .Must(Converter.IsHexString)
            .WithMessage("16進数でなければいけません");
    }
    
    public static IRuleBuilderOptions<T, string> IsSymbolAddress<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .Must(address =>
            {
                try
                {
                    return (address[0].ToString() == "N" || address[0].ToString() == "T") && address.Length == 39;
                }
                catch
                {
                    return false;
                }
            })
            .WithMessage("正しいAddressの形式ではありません");
    }
}

