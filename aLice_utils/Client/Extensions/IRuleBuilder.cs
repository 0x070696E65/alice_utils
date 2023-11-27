using System.Text.RegularExpressions;
using aLice_utils.Shared.Models;
using CatSdk.Utils;
using FluentValidation;

namespace aLice_utils.Client.Extensions;

public static class IRuleBuilder
{
    public static IRuleBuilderOptions<T, string> IsUnder1023Byte<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .Must((str) => Converter.Utf8ToBytes(str).Length < 1024)
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
    
    public static IRuleBuilderOptions<T, string> IsDeadline<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .Must((str) =>
            {
                try
                {
                    var t = int.Parse(str);
                    return t is >= 10 and <= 7200;   
                } catch {
                    return false;
                }
            })
            .WithMessage("Deadlineは10秒以上7200秒以下でなければいけません");
    }
    
    public static IRuleBuilderOptions<T, string> IsMosaicId16<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .Must(id => id.Length == 16)
            .WithMessage("モザイクIDは16文字でなければいけません");
    }
    public static IRuleBuilderOptions<T, string> IsMosaicIdHex<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .Must(Converter.IsHexString)
            .WithMessage("モザイクIDは16進数でなければいけません");
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

