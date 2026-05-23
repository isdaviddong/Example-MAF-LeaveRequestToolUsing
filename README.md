# Example-MAF-LeaveRequestToolUsing

以 C# 實作的對話式「請假助理」範例，使用 Microsoft Agents + OpenAI Responses API，透過 function calling 呼叫本地工具方法來完成：

- 員工請假申請
- 員工已請假天數查詢
- 台灣時區日期取得

## 功能說明

此範例在對話中會引導蒐集必要資訊，資訊完整後自動呼叫工具方法：

- `LeaveRequest`: 建立假單
- `GetLeaveRecordAmount`: 查詢已請假天數
- `GetCurrentDate`: 取得台灣時區當日日期

## 專案需求

- .NET SDK 10（目前專案目標框架為 `net10.0`）
- OpenAI API Key
- 可連線至 NuGet 與 OpenAI API 的網路環境

## 套件依賴

目前專案使用的主要 NuGet 套件：

- `Microsoft.Agents.AI`
- `Microsoft.Agents.AI.OpenAI`
- `Microsoft.Extensions.AI`
- `OpenAI`
- `Azure.AI.Projects`（目前程式碼未啟用 Azure 版本建立流程）
- `Azure.Identity`（目前程式碼未啟用 Azure 版本建立流程）

## 快速開始

1. 還原套件

```bash
dotnet restore
```

2. 執行程式

```bash
dotnet run
```

3. 輸入 OpenAI API Key

程式啟動後會提示：

```text
請輸入 OpenAI API Key：
```

輸入後即可開始對話。

## 對話範例

```text
用戶 > 我要請假
AI助理 > 好的，請問你想從哪一天開始請假？

用戶 > 2026-05-26
AI助理 > 了解，預計請幾天？

用戶 > 2
AI助理 > 請問請假事由、代理人與請假者姓名是？

用戶 > 家庭因素，代理人是 Eric，我是 David
AI助理 > 已為你建立請假申請。
```

## 程式入口

- `Program.cs`
	- 建立 `AIAgent`
	- 設定系統指令（角色、語言、流程）
	- 註冊工具方法
	- 進入互動式命令列對話

## 常見問題

### 1) 啟動後立即結束

若 API Key 留空，程式會顯示「未提供 API Key，程式結束。」這是預期行為。

### 2) 套件還原失敗

請先確認：

- NuGet 來源可連線
- 本機 .NET SDK 版本支援 `net10.0`

### 3) 呼叫模型失敗

請檢查：

- OpenAI API Key 是否正確
- API Key 是否具備可用額度與對應模型權限

## 延伸建議

- 將 API Key 改為環境變數讀取，避免硬編碼
- 將假單資料改為資料庫持久化
- 補上單元測試與整合測試