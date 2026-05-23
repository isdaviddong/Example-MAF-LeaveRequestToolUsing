using System;
using System.ComponentModel;
using Azure.AI.Projects;
using Azure.Identity;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using OpenAI;
using OpenAI.Responses;

#pragma warning disable MAAI001, OPENAI001

internal class Program
{
    private static async Task Main(string[] args)
    {
        var model = "gpt-4.1";
        string openAIApiKey = "REPLACE_WITH_YOUR_OPENAI_API_KEY";

        if (openAIApiKey == "REPLACE_WITH_YOUR_OPENAI_API_KEY")
        {
            Console.Write("請輸入 OpenAI API Key：");
            var inputKey = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(inputKey))
            {
                Console.WriteLine("未提供 API Key，程式結束。");
                return;
            }

            openAIApiKey = inputKey.Trim();
        }

        var leaveTools = new LeaveRequestTools();
         AIAgent agent = new OpenAIClient(openAIApiKey)
            .GetResponsesClient()
            .AsAIAgent(
                model: model,
                instructions:
                """
                你是企業的請假助理，可以協助員工進行請假，或是查詢請假天數等功能。

                若員工需要請假，你需要蒐集：
                - 請假起始日期
                - 天數
                - 請假事由
                - 代理人
                - 請假者姓名

                資訊完整後，呼叫 LeaveRequest。

                若員工需要查詢請假天數，你需要蒐集請假者姓名，
                然後呼叫 GetLeaveRecordAmount。

                所有對談請用正體中文回答。
                請以口語化的方式回答，要適合對談機器人的角色。
                """,
                tools:
                [
                    AIFunctionFactory.Create(leaveTools.GetLeaveRecordAmount),
                    AIFunctionFactory.Create(leaveTools.LeaveRequest),
                    AIFunctionFactory.Create(leaveTools.GetCurrentDate)
                ]);


        var session = await agent.CreateSessionAsync();

        Console.Write("用戶 > ");
        string? userInput;

        while (!string.IsNullOrWhiteSpace(userInput = Console.ReadLine()))
        {
            var response = await agent.RunAsync(userInput, session);

            Console.WriteLine("AI助理 > " + response.Text);
            Console.Write("\n用戶 > ");
        }
    }
}

public class LeaveRequestTools
{
    [Description("取得指定員工已請假的天數")]
    public int GetLeaveRecordAmount(
        [Description("要查詢請假天數的員工名稱")]
        string employeeName)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"\n[action] 查詢 {employeeName} 請假天數。\n");
        Console.ResetColor();

        return employeeName.ToLower() switch
        {
            "david" => 5,
            "eric" => 8,
            _ => 3
        };
    }

    [Description("建立一筆員工請假申請")]
    public bool LeaveRequest(
        [Description("請假起始日期")]
        DateTime startDate,

        [Description("請假天數")]
        string days,

        [Description("請假事由")]
        string reason,

        [Description("代理人")]
        string substitute,

        [Description("請假者姓名")]
        string employeeName)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"""
        
        [action] 建立假單:
        {employeeName} 請假 {days} 天，
        從 {startDate:yyyy-MM-dd} 開始，
        事由為 {reason}，
        代理人 {substitute}

        """);
        Console.ResetColor();

        return true;
    }

    [Description("取得今天日期，台灣時區")]
    public DateTime GetCurrentDate()
    {
        return DateTime.UtcNow.AddHours(8);
    }
}