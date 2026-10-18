# SQL 语法知识点

SQL（Structured Query Language）是用于管理和操作关系型数据库的标准语言，按功能可分为四类：

- **数据查询（DQL）**：`select`，检索数据，不改变表中内容。
- **数据操作（DML）**：`insert`、`update`、`delete`，对表中数据进行增、改、删。
- **数据定义（DDL）**：`create`、`alter`、`drop`，定义或修改数据库及表的结构。
- **数据控制（DCL）**：`grant`、`revoke`，管理访问权限（日常开发较少接触，本文不展开）。

本文以 SQL Server（T-SQL）为准，按查询、数据操作、表结构定义、常用函数、进阶主题的顺序整理常用语法。SQL 关键字不区分大小写，本文统一使用小写。

## 1. 示例数据

以下示例均基于两张表。

**Students（学员表）**

| StudentId | StudentName | Gender | Age | ClassId |
| --------- | ----------- | ------ | --- | ------- |
| 1001      | 张三        | 男     | 20  | 1       |
| 1002      | 李四        | 女     | 22  | 1       |
| 1003      | 王五        | 男     | 25  | 2       |
| 1004      | 赵六        | 女     | 19  | NULL    |

**StudentClass（班级表）**

| ClassId | ClassName |
| ------- | --------- |
| 1       | 软件1班   |
| 2       | 软件2班   |

学员表的 `ClassId` 引用班级表的 `ClassId`。赵六的 `ClassId` 为 NULL，表示尚未分班。

## 2. 数据查询（SELECT）

### 2.1 基本查询

`select` 指定要返回的列，`from` 指定数据来源的表。

```sql
-- 查询指定列
select StudentName, Age from Students;

-- 查询所有列（正式代码中建议明确列出列名）
select * from Students;

-- as 为列指定别名（as 可省略）
select StudentName as 姓名, Age as 年龄 from Students;

-- distinct 去除重复行
select distinct ClassId from Students;

-- top N 只返回前 N 行
select top 2 StudentName from Students;
```

### 2.2 条件过滤（WHERE）

`where` 子句按条件筛选行，只保留满足条件的记录。

#### 2.2.1 比较与逻辑运算

| 运算符                | 含义                                 |
| --------------------- | ------------------------------------ |
| `=` `>` `>=` `<` `<=` | 等于、大于、大于等于、小于、小于等于 |
| `<>` 或 `!=`          | 不等于                               |
| `and`                 | 并且，多个条件同时成立               |
| `or`                  | 或者，任一条件成立即可               |
| `not`                 | 取反                                 |

```sql
-- 查询 20 岁及以上的学员
select StudentName, Age from Students where Age >= 20;

-- 多条件组合
where Age >= 20 and Gender = N'男'
where ClassId = 1 or ClassId = 2
```

注意：`and` 的优先级高于 `or`，混合使用时建议使用括号明确运算顺序，如 `where (ClassId = 1 or ClassId = 2) and Age > 20`。

修改数据时的查重通常需要排除记录本身，写法为 `where StudentIdNo = '...' and StudentId <> 当前Id`。

#### 2.2.2 范围与集合（BETWEEN / IN）

```sql
-- between 闭区间，包含两端，等价于 Age >= 20 and Age <= 25
where Age between 20 and 25

-- in 匹配列举的任一值，等价于 ClassId = 1 or ClassId = 2
where ClassId in (1, 2)
```

#### 2.2.3 模糊查询（LIKE）

`like` 用于按模式匹配字符串，支持两种通配符：

- `%`：匹配任意长度的任意字符（包括 0 个）。
- `_`：匹配恰好一个任意字符。

```sql
where StudentName like N'张%'       -- 以"张"开头：张三、张三丰
where StudentName like N'%三'       -- 以"三"结尾
where StudentName like N'%三%'      -- 包含"三"
where StudentName like N'张_'       -- 姓张且姓名共两个字
```

#### 2.2.4 空值判断（NULL）

NULL 表示未知或缺失的值，不能使用等号比较，必须使用 `is null` / `is not null` 判断。

```sql
where ClassId is null              -- 查询尚未分班的学员
where ClassId is not null          -- 查询已分班的学员
where ClassId = null               -- 错误写法，永远匹配不到任何行
```

### 2.3 排序与分页（ORDER BY）

`order by` 对结果排序，`asc` 为升序（默认，可省略），`desc` 为降序。支持多列排序，先按第一列排，相同再按第二列排。

```sql
-- 按年龄降序
select StudentName, Age from Students order by Age desc;

-- 先按班级升序，同班再按年龄降序
order by ClassId asc, Age desc;
```

SQL Server 2012 及以上版本使用 `offset` / `fetch` 实现分页：

```sql
-- 跳过 10 行，取 5 行，即每页 5 条时的第 3 页
select StudentName, Age from Students
order by StudentId
offset 10 rows fetch next 5 rows only;
```

换算规则为 `offset (页码-1)×每页条数 rows fetch next 每页条数 rows only`。注意 `offset` / `fetch` 必须与 `order by` 配合使用。

### 2.4 聚合函数

聚合函数将多行数据计算为单个结果，常用函数如下：

| 函数                  | 作用                   |
| --------------------- | ---------------------- |
| `count(*)`            | 统计行数               |
| `count(列名)`         | 统计该列非 NULL 的行数 |
| `sum(列)`             | 求和                   |
| `avg(列)`             | 平均值                 |
| `max(列)` / `min(列)` | 最大值 / 最小值        |

```sql
select count(*) as 总人数, avg(Age) as 平均年龄, max(Age) as 最大年龄
from Students;
```

`count(*)` 结合 `where` 可用于判断记录是否存在：返回 0 表示不存在，大于 0 表示已存在。新增数据前的唯一性校验（如身份证号、用户名查重）通常采用该方式。

### 2.5 分组（GROUP BY / HAVING）

`group by` 将数据按指定列分组，再对每组分别进行聚合统计。

```sql
-- 统计每个班级的学员人数
select ClassId, count(*) as 人数
from Students
group by ClassId;
```

注意：`select` 中出现的非聚合列必须出现在 `group by` 中，否则只能作为聚合函数的参数。因为分组之后每组只保留组标识和统计值，无法取组内单行的明细数据。

`having` 用于在分组之后对组进行筛选：

```sql
-- 查询人数超过 1 人的班级
select ClassId, count(*) as 人数
from Students
group by ClassId
having count(*) > 1;
```

`where` 与 `having` 的区别：`where` 在分组前筛选行，`having` 在分组后筛选组。聚合函数只能出现在 `having` 中，不能出现在 `where` 中。

### 2.6 多表连接（JOIN）

学员表只存储班级编号 `ClassId`，班级名称在班级表中。连接查询通过外键将两张表组合为一个结果集，一次查出学员及其班级名称。

#### 2.6.1 INNER JOIN（内连接）

内连接只保留两表都匹配上的行。

```sql
select s.StudentName, s.Age, c.ClassName
from Students s
inner join StudentClass c on s.ClassId = c.ClassId;
```

执行结果：

| StudentName | Age | ClassName |
| ----------- | --- | --------- |
| 张三        | 20  | 软件1班   |
| 李四        | 22  | 软件1班   |
| 王五        | 25  | 软件2班   |

说明：

- `from Students s` 中的 `s` 为表别名，用于简化书写。两表存在同名列时，必须使用别名前缀指明列的归属，如 `s.ClassId`。
- `on` 指定连接条件（外键 = 主键）。
- 赵六的 `ClassId` 为 NULL，无法匹配任何班级，因此不出现在结果中。

#### 2.6.2 LEFT JOIN（左连接）

左连接保留左表的全部行，右表无匹配时对应列填充 NULL。

```sql
select s.StudentName, c.ClassName
from Students s
left join StudentClass c on s.ClassId = c.ClassId;
```

执行结果：

| StudentName | ClassName |
| ----------- | --------- |
| 张三        | 软件1班   |
| 李四        | 软件1班   |
| 王五        | 软件2班   |
| 赵六        | NULL      |

在左连接基础上增加 `where c.ClassId is null` 条件，可筛出左表中未匹配上的行，例如查询所有尚未分班的学员。

`right join` 与 `left join` 互为镜像，`full join` 保留两表全部行，实际开发中使用较少。

### 2.7 子查询

子查询是嵌套在另一个查询内部的查询，括号内的子查询先执行，其结果作为外层查询的输入。

```sql
-- 查询年龄大于平均年龄的学员
select StudentName, Age from Students
where Age > (select avg(Age) from Students);
```

子查询常见使用位置：

```sql
-- 1. where 中配合 in：查询"软件1班"的所有学员
select StudentName from Students
where ClassId in (select ClassId from StudentClass where ClassName = N'软件1班');

-- 2. from 中作为派生表（必须指定别名）
select t.ClassId, t.人数
from (select ClassId, count(*) as 人数 from Students group by ClassId) t
where t.人数 > 1;

-- 3. where 中配合 exists 做存在性判断
select StudentName from Students s
where exists (select 1 from Scores sc where sc.StudentId = s.StudentId);
```

多数子查询可改写为 JOIN，两者都可行时优先使用 JOIN，可读性和性能通常更好。

### 2.8 联合查询（UNION）

`union` 将多个结构相同的结果集纵向合并。

```sql
select StudentName, Age from Students where ClassId = 1
union
select StudentName, Age from Students where ClassId = 2;
```

- `union` 自动去除重复行，`union all` 保留全部行。确认无重复数据时使用 `union all` 性能更好。
- 各结果集的列数必须一致，对应列的数据类型必须兼容。

## 3. 数据操作（DML）

### 3.1 插入（INSERT）

```sql
-- 单行插入：列名与值一一对应
insert into Students (StudentName, Gender, Age, ClassId)
values (N'孙七', N'女', 21, 2);

-- 多行插入
insert into Students (StudentName, Gender, Age)
values (N'钱八', N'男', 23), (N'周九', N'女', 22);

-- 将查询结果插入另一张表（常用于数据备份或迁移）
insert into StudentsBackup (StudentName, Age)
select StudentName, Age from Students where Age > 20;
```

注意：自增列（如 `StudentId`）不应出现在列名列表中，由数据库自动生成。语句执行后返回受影响行数，大于 0 表示插入成功。

### 3.2 更新（UPDATE）

```sql
update Students
set Age = 26, ClassId = 2
where StudentId = 1001;
```

`set` 后多个列使用逗号分隔。注意：若省略 `where` 条件，将更新表中的全部记录，执行更新操作前务必确认条件无误。

### 3.3 删除（DELETE）

```sql
delete from Students where StudentId = 1001;
```

同样必须携带 `where` 条件，否则清空整表。若待删除的记录被其他表通过外键引用，数据库会拒绝删除并报 `FOREIGN KEY constraint` 错误，需要在程序中捕获该异常并给出相应提示。

### 3.4 DELETE 与 TRUNCATE 的区别

```sql
truncate table Students;   -- 清空整表
```

| 对比项   | DELETE                    | TRUNCATE     |
| -------- | ------------------------- | ------------ |
| 条件     | 可携带 `where` 删除部分行 | 只能清空整表 |
| 自增列   | 不重置                    | 重置为初始值 |
| 执行速度 | 较慢（逐行记录日志）      | 快           |

## 4. 表结构定义（DDL）

### 4.1 数据库操作

```sql
create database StudentManageDB;    -- 创建数据库
use StudentManageDB;                -- 切换到指定数据库
drop database StudentManageDB;      -- 删除数据库
```

### 4.2 表操作

```sql
create table Students
(
    StudentId   int identity(1000,1) primary key,         -- 自增主键：从 1000 开始，每次加 1
    StudentName nvarchar(20) not null,
    Gender      nchar(1) not null,
    StudentIdNo varchar(18) not null unique,
    Age         int check(Age between 0 and 150),
    PhoneNumber varchar(11),
    StudentAddress nvarchar(200) default N'地址不详',
    ClassId     int foreign key references StudentClass(ClassId)
);

-- 修改表结构
alter table Students add Email varchar(50);            -- 添加列
alter table Students alter column Email varchar(100);  -- 修改列类型
alter table Students drop column Email;                -- 删除列

drop table Students;                                   -- 删除表
```

### 4.3 常用数据类型

| 类型            | 用途                         | 示例                     |
| --------------- | ---------------------------- | ------------------------ |
| `int`           | 整数                         | 年龄、数量               |
| `bigint`        | 大整数                       | 超大编号                 |
| `decimal(18,2)` | 精确小数，共 18 位、2 位小数 | 金额                     |
| `nvarchar(n)`   | 变长 Unicode 字符串          | 姓名、地址等含中文的内容 |
| `varchar(n)`    | 变长非 Unicode 字符串        | 身份证号、手机号         |
| `nchar(n)`      | 定长 Unicode 字符串          | 性别 `nchar(1)`          |
| `datetime`      | 日期和时间                   | 出生日期                 |
| `date`          | 仅日期                       | 入学日期                 |
| `bit`           | 0/1，对应 C# 的 bool         | 是否毕业                 |

### 4.4 约束

| 约束          | 作用                                                   |
| ------------- | ------------------------------------------------------ |
| `primary key` | 主键，唯一且非空，一张表只能有一个，用于准确定位每一行 |
| `foreign key` | 外键，取值必须在被引用表的主键中存在，保证引用的完整性 |
| `unique`      | 唯一约束，列值不允许重复                               |
| `not null`    | 非空约束，插入时必须提供值                             |
| `default`     | 默认值，插入时未提供值则自动填充                       |
| `check`       | 检查约束，列值必须满足指定条件                         |

外键约束同时影响删除操作：被其他表引用的记录不能直接删除。

## 5. 常用函数

### 5.1 字符串函数

| 函数                       | 作用                       | 示例                              |
| -------------------------- | -------------------------- | --------------------------------- |
| `len(s)`                   | 返回长度                   | `len('abc')` → 3                  |
| `upper(s)` / `lower(s)`    | 转换大小写                 | `upper('abc')` → 'ABC'            |
| `ltrim(s)` / `rtrim(s)`    | 去除左/右空格              | `ltrim('  a')` → 'a'              |
| `substring(s, 起点, 长度)` | 截取子串                   | `substring('abcdef',2,3)` → 'bcd' |
| `replace(s, 旧值, 新值)`   | 替换                       | `replace('aab','a','x')` → 'xxb'  |
| `charindex(子串, s)`       | 返回子串位置，未找到返回 0 | `charindex('c','abc')` → 3        |
| `concat(s1, s2, ...)`      | 拼接字符串                 | `concat('a','b')` → 'ab'          |

注意：SQL 中字符串位置从 1 开始计数。

### 5.2 日期函数

| 函数                              | 作用             | 示例                                         |
| --------------------------------- | ---------------- | -------------------------------------------- |
| `getdate()`                       | 返回当前日期时间 | `select getdate()`                           |
| `year(d)` / `month(d)` / `day(d)` | 取年/月/日       | `year(Birthday)`                             |
| `dateadd(单位, 数量, d)`          | 日期加减         | `dateadd(day, 7, getdate())` → 7 天后        |
| `datediff(单位, d1, d2)`          | 计算日期差       | `datediff(year, Birthday, getdate())` → 周岁 |
| `format(d, '格式')`               | 格式化输出       | `format(getdate(), 'yyyy-MM-dd')`            |

### 5.3 类型转换

```sql
cast('123' as int)                        -- 字符串转数值
cast(Age as varchar(10)) + N'岁'          -- 数值转字符串后拼接
convert(varchar(10), getdate(), 120)      -- 日期转字符串，120 对应格式 yyyy-mm-dd hh:mi:ss
```

### 5.4 空值处理

```sql
-- isnull(列, 替代值)：列为 NULL 时返回替代值
select StudentName, isnull(PhoneNumber, '未填写') from Students;

-- coalesce(值1, 值2, ...)：返回第一个非 NULL 的值
select coalesce(PhoneNumber, Email, '无任何联系方式') from Students;
```

## 6. SQL Server 特性

### 6.1 Unicode 字符串（N 前缀）

```sql
where StudentName = N'张三'    -- 正确：按 Unicode 处理，中文不乱码
where StudentName = '张三'     -- 可能乱码或匹配失败
```

`N` 前缀表示字符串按 `nvarchar`（Unicode）处理。凡可能包含中文的值都应添加 `N` 前缀。

### 6.2 引号规则

| 类型                 | 写法                             | 示例                          |
| -------------------- | -------------------------------- | ----------------------------- |
| 字符串、日期         | 使用单引号包裹                   | `'2000-01-15'`、`'110101...'` |
| 数值                 | 不加引号                         | `Age = 25`                    |
| 标识符（表名、列名） | 一般不加，与关键字冲突时用方括号 | `[Order]`                     |

注意：身份证号等编号虽然以数字为主，但可能包含字母 `X`，应按字符串处理并加引号。

### 6.3 自增列（IDENTITY）

```sql
StudentId int identity(1000, 1) primary key   -- 种子为 1000，增量为 1
```

插入数据时不能为自增列赋值。获取刚插入记录的自增编号可使用 `select scope_identity()`。

## 7. 进阶主题

### 7.1 事务（Transaction）

事务将多条 SQL 语句绑定为一个整体，要么全部提交，要么全部回滚，用于保证数据一致性。典型场景为银行转账：扣款和入账必须同时成功或同时失败。

```sql
begin try
    begin transaction;
    update Accounts set Balance = Balance - 100 where AccountId = 1;
    update Accounts set Balance = Balance + 100 where AccountId = 2;
    commit;        -- 全部成功：提交，修改永久生效
end try
begin catch
    rollback;      -- 任一步骤失败：回滚，撤销所有修改
end catch
```

### 7.2 SQL 逻辑执行顺序

SQL 语句的书写顺序与数据库的实际执行顺序不同：

```
from → join/on → where → group by → having → select → distinct → order by → top/offset
```

理解执行顺序可以解释两个常见现象：

- `where` 先于 `select` 执行，因此 `where` 中不能使用 `select` 定义的列别名。
- 聚合发生在 `group by` 阶段，晚于 `where`，因此聚合函数不能出现在 `where` 中，只能使用 `having` 筛选。

### 7.3 ADO.NET 执行方式

在 C# 中通过 `SqlCommand` 执行 SQL，根据返回结果的形态选择执行方法：

| 方法                | 适用 SQL                        | 返回值                  | 典型场景                 |
| ------------------- | ------------------------------- | ----------------------- | ------------------------ |
| `ExecuteNonQuery()` | insert / update / delete        | 受影响行数              | 增删改数据               |
| `ExecuteScalar()`   | 返回单个值的 select（如 count） | object，第一行第一列    | 查重、统计、获取自增编号 |
| `ExecuteReader()`   | 返回多行多列的 select           | SqlDataReader，逐行读取 | 查询列表、登录验证       |

使用建议：

- `ExecuteReader` 传入 `CommandBehavior.CloseConnection`，DataReader 关闭时连接会自动关闭，调用方只需关闭 Reader。
- 连接字符串统一存放在配置文件（App.config）中，通过 `ConfigurationManager.ConnectionStrings` 读取，避免硬编码。

### 7.4 SQL 注入与参数化查询

使用 `string.Format` 拼接 SQL 存在注入风险。以登录查询为例：

```sql
select AdminName from Admins where LoginId = {0} and LoginPwd = '{1}'
```

若密码输入 `' or '1'='1`，拼接后的条件恒为真，攻击者无需正确密码即可通过验证。

标准做法是使用参数化查询，将参数值与 SQL 语句分离，由数据库驱动完成转义：

```csharp
SqlCommand cmd = new SqlCommand("select AdminName from Admins where LoginId=@id and LoginPwd=@pwd", conn);
cmd.Parameters.AddWithValue("@id", objAdmin.LoginId);
cmd.Parameters.AddWithValue("@pwd", objAdmin.LoginPwd);
```

生产环境应一律使用参数化查询，禁止通过字符串拼接构造 SQL。
