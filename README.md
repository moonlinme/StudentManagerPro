# StudentManagerPro

基于 C# WinForms 的学员管理系统，采用三层结构（UI / DAL / Models），并使用委托与事件实现窗体间通信。

## 技术栈

- .NET Framework / WinForms
- SQL Server（数据访问）
- Visual Studio 2017+（打开 `StudentManagerPro.sln`）

## 项目结构

| 项目 | 说明 |
| ---- | ---- |
| `StudentManagerPro` | 界面层：登录（FrmUserLogin）、主窗体（FrmMain）、学员管理（FrmStudentManage）、添加学员（FrmAddStudent）、编辑学员（FrmEditStudent） |
| `DAL` | 数据访问层：StudentService、StudentClassService、SysAdminService、ScoreListService，底层数据库操作封装在 SQLHelper |
| `Models` | 数据模型层：SysAdmin、Student、StudentClass、ScoreList 等实体类 |

## 窗体通信实现（学习示例）

项目内包含三种委托/事件通信写法，均在 `Program.cs` 与各窗体中：

1. **定向传递 A → B → C → A（实例委托）**
   - `FrmData` 同一个类 new 出三个窗体实例
   - 每个实例持有 `SendTo` 委托指向下家的 `ReceiveData`
   - B 开启 `AutoForward`，收到 A 的数据后自动转发给 C
   - 启动入口：`Program.StartDemo()`

2. **全局事件（1 发布者 → N 订阅者）**
   - `Program.StudentChanged` 静态事件
   - `FrmAddStudent` 添加学员成功后发布，`FrmStudentManage` 订阅并刷新

3. **消息中心（N 发布者 → N 订阅者，观察者模式）**
   - `IMessageReceiver` 接口 + `MessageCenter` 静态类
   - 各窗体订阅/广播，形成网状通信

## 文档

| 文档 | 内容 |
| ---- | ---- |
| [docs/项目架构.md](docs/项目架构.md) | 三层结构与窗体通信设计说明 |
| [docs/委托.md](docs/委托.md) | 委托/事件的概念讲解 |
| [docs/委托定向通信.md](docs/委托定向通信.md) | A → B → C → A 定向传递的实现笔记 |
| [docs/C#语法知识点.md](docs/C%23语法知识点.md) | C# 常用语法整理 |
| [docs/SQL/SQL语法知识点.md](docs/SQL/SQL语法知识点.md) | SQL 常用语法整理 |

## 运行

1. 用 Visual Studio 打开 `StudentManagerPro.sln`
2. 配置 `DAL` 中的数据库连接字符串
3. F5 运行
