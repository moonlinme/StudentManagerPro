# C# 语法知识点

C# 是微软基于 .NET 平台开发的强类型、面向对象编程语言。本文以 WinForms 开发为背景整理常用语法，部分示例标注了所在的文件名（虚拟），用于说明代码在分层结构中的位置。

注意：C# 严格区分大小写；每条语句以分号 `;` 结尾。

## 1. 程序基本结构

### 1.1 命名空间与类

C# 程序由命名空间（namespace）组织，命名空间下定义类（class），类中包含字段、属性、方法等成员。命名空间用于按功能分组代码、避免类名冲突。

```csharp
using System;                 // 引入命名空间，之后可直接使用其中的类

namespace StudentManagerPro   // 定义命名空间
{
    class Program             // 定义类
    {
        static void Main()    // 程序入口方法，程序从这里开始执行
        {
            Console.WriteLine("Hello");
        }
    }
}
```

### 1.2 注释

```csharp
// 单行注释

/* 多行注释 */

/// <summary>
/// XML 文档注释，写在类或方法上方，鼠标悬停时 IDE 会显示
/// </summary>
```

## 2. 变量与数据类型

### 2.1 变量与常量

变量是存储数据的容器，声明时必须指定类型，类型一旦确定不可更改（强类型）。

```csharp
int age = 20;              // 声明并赋值
string name;               // 先声明
name = "张三";             // 后赋值
const double PI = 3.14159; // 常量，声明时必须赋值，之后不可修改
```

### 2.2 值类型与引用类型

- **值类型（Value Type）**：变量直接存数据，赋值时复制副本，互不影响。如 `int`、`double`、`bool`、`struct`。
- **引用类型（Reference Type）**：变量存对象的地址，赋值时复制地址，两个变量指向同一对象。如 `string`、数组、class。

```csharp
int a = 10;
int b = a;      // b 是 a 的副本
b = 20;         // a 仍为 10

int[] arr1 = { 1, 2, 3 };
int[] arr2 = arr1;   // arr2 与 arr1 指向同一个数组
arr2[0] = 100;       // arr1[0] 也变为 100
```

与 JavaScript 中基本类型和对象的赋值行为一致。

### 2.3 常用数据类型

| 类型         | 说明         | 示例                                               |
| ---------- | ---------- | ------------------------------------------------ |
| `int`      | 32 位整数     | `int age = 20;`                                  |
| `long`     | 64 位整数     | `long id = 123456789012L;`                       |
| `double`   | 双精度浮点数     | `double score = 95.5;`                           |
| `decimal`  | 高精度小数，用于金额 | `decimal price = 9.9M;`                          |
| `bool`     | 布尔值        | `bool isPassed = true;`                          |
| `char`     | 单个字符，单引号   | `char gender = '男';`                             |
| `string`   | 字符串，双引号    | `string name = "张三";`                            |
| `DateTime` | 日期时间       | `DateTime birthday = new DateTime(2000, 1, 15);` |

注意：`string` 是引用类型，但具有不可变性，每次修改都会生成新对象。

### 2.4 var 隐式类型

`var` 由编译器根据初始值推断类型，声明时必须初始化。编译后类型即固定，不是动态类型，更接近 TypeScript 的类型推断。

```csharp
var age = 20;                 // 推断为 int
var list = new List<int>();   // 推断为 List<int>
```

### 2.5 可空类型

值类型默认不能为 `null`，但数据库中的数值列可能为空。类型后加 `?` 即可声明可空类型。

```csharp
int? classId = null;
if (classId.HasValue)
{
    int id = classId.Value;
}

// ?? 空合并运算符：左边为 null 时取右边的默认值
int id2 = classId ?? 0;
```

### 2.6 类型转换

```csharp
// 隐式转换：小范围到大范围，自动完成
int a = 10;
double b = a;

// 显式转换（强制转换）：可能丢失精度
double c = 9.8;
int d = (int)c;            // d = 9，小数部分截断

// Parse：字符串转数值，格式错误抛异常
int e = int.Parse("123");

// TryParse：失败返回 false 不抛异常，处理用户输入时推荐
if (int.TryParse(txtAge.Text, out int f)) { }

// Convert：通用转换，null 转数值返回 0
// 示例：StudentService.cs 中读取数据库查询结果
int g = Convert.ToInt32(reader["Age"]);

// ToString：任意类型转字符串
string s = 123.ToString();
```

选择建议：来源可靠用 `Parse`，用户输入用 `TryParse`，数据库查询结果用 `Convert`。

### 2.7 枚举（Enum）

枚举将一组相关常量命名，避免使用难以理解的"魔法值"。

```csharp
public enum Gender { 男, 女 }

Gender g = Gender.男;
```

### 2.8 装箱与拆箱

装箱（Boxing）是将值类型转换为 `object` 的过程，拆箱（Unboxing）则相反。频繁装箱拆箱影响性能，使用泛型集合（`List<int>`）代替非泛型集合（`ArrayList`）可避免。

```csharp
int a = 10;
object obj = a;     // 装箱
int b = (int)obj;   // 拆箱，必须强制转换为原类型
```

### 2.9 相等比较（== 与 Equals）

| 类型       | `==` 的行为     | `Equals()` 的行为  |
| -------- | ------------ | --------------- |
| 值类型      | 比较值          | 比较值             |
| `string` | 比较内容（运算符被重载） | 比较内容            |
| 其他引用类型   | 比较引用         | 默认比较引用，可重写为比较内容 |

```csharp
string s1 = "abc";
string s2 = "abc";
Console.WriteLine(s1 == s2);     // True，string 的 == 比较内容

Student stu1 = new Student();
Student stu2 = stu1;             // 指向同一对象
Student stu3 = new Student();
Console.WriteLine(stu1 == stu2); // True
Console.WriteLine(stu1 == stu3); // False
```

## 3. 运算符

| 分类     | 运算符                         |
| ------ | --------------------------- |
| 算术     | `+` `-` `*` `/` `%`（取余）     |
| 比较     | `==` `!=` `>` `>=` `<` `<=` |
| 逻辑     | `&&`（并且）`\|\|`（或者）`!`（取反）   |
| 赋值     | `=` `+=` `-=` `*=` `/=`     |
| 自增自减   | `++` `--`                   |
| 条件（三元） | `条件 ? 值1 : 值2`              |

```csharp
string level = age >= 18 ? "成年" : "未成年";
```

注意：`&&` 和 `||` 具有短路特性：左边已能确定结果时，右边不再执行。事件触发前的判空 `if (handler != null && handler())` 正是利用了这一特性。

## 4. 流程控制

### 4.1 条件语句（if / switch）

```csharp
if (age >= 18) { }
else if (age >= 12) { }
else { }

// switch：按固定值分支
switch (classId)
{
    case 1:
        Console.WriteLine("软件1班");
        break;
    default:                  // 所有 case 都不匹配时执行
        Console.WriteLine("未知班级");
        break;
}
```

注意：C# 的 `case` 必须以 `break` 等跳转语句结束，不能像 JavaScript 那样自动贯穿。

### 4.2 循环语句

```csharp
// for：已知循环次数
for (int i = 0; i < 10; i++) { }

// foreach：遍历集合，最常用
foreach (Student stu in studentList) { }

// while：先判断后执行
// 示例：StudentService.cs 中逐行读取查询结果
while (reader.Read()) { }

// do-while：先执行一次再判断
do { } while (condition);
```

### 4.3 跳转语句

- `break`：退出当前整个循环。
- `continue`：跳过本次循环，进入下一次。
- `return`：结束当前方法并返回结果。

## 5. 数组与集合

### 5.1 数组（Array）

长度创建时确定，之后不可改变。

```csharp
int[] scores = new int[5];            // 长度为 5
string[] names = { "张三", "李四" };  // 声明同时初始化
int first = scores[0];                // 下标从 0 开始
int len = scores.Length;
```

### 5.2 List\<T>（列表）

长度可变的泛型集合，最常用。元素个数用 `Count`（数组用 `Length`）。

```csharp
// 示例：StudentService.cs 中将查询结果装入 List 返回
List<Student> list = new List<Student>();
list.Add(new Student());          // 尾部添加
list.Remove(stu);                 // 删除指定元素
int count = list.Count;           // 元素个数
Student first = list[0];          // 按下标访问
```

### 5.3 Dictionary\<K,V>（字典）

键值对集合，按键快速查找值，相当于前端的 Map。

```csharp
Dictionary<int, string> classMap = new Dictionary<int, string>();
classMap.Add(1, "软件1班");
string name = classMap[1];             // 按键取值
bool hasKey = classMap.ContainsKey(1); // 判断键是否存在
```

注意：键不能重复；按键取不存在的值会抛异常，应先 `ContainsKey` 判断。

### 5.4 泛型（Generics）

泛型允许定义类、方法时不指定具体类型，用类型参数 `T` 占位，使用时再确定。`List<T>` 中的 `T` 即类型参数。

```csharp
// 定义泛型类：同一套逻辑适配不同类型
public class Box<T>
{
    public T Content { get; set; }
}

// 使用时在尖括号中指定具体类型
Box<int> intBox = new Box<int>() { Content = 10 };
Box<string> strBox = new Box<string>() { Content = "文本" };
```

不用泛型只能把一切都当 `object` 处理，存入装箱、取出强转，类型错误到运行时才暴露。泛型让编译器在编译期检查类型，安全且高效。

### 5.5 LINQ 基础查询

LINQ 为集合提供类似 SQL 的查询能力：

| 方法                                        | 作用                | 类比 SQL         |
| ----------------------------------------- | ----------------- | -------------- |
| `Where(...)`                              | 条件筛选              | `where`        |
| `Select(...)`                             | 投影取字段             | `select 列名`    |
| `FirstOrDefault(...)`                     | 取第一个满足条件的，无则返回默认值 | `top 1`        |
| `Any(...)`                                | 是否存在满足条件的         | `count(*) > 0` |
| `OrderBy(...)` / `OrderByDescending(...)` | 升序 / 降序           | `order by`     |
| `Count(...)`                              | 统计个数              | `count(*)`     |

```csharp
// 筛选年龄大于 20 的学员
List<Student> result = studentList.Where(s => s.Age > 20).ToList();

// 查找学号为 1001 的学员，找不到返回 null
Student stu = studentList.FirstOrDefault(s => s.StudentId == 1001);

// 判断是否存在 2 班的学员
bool exists = studentList.Any(s => s.ClassId == 2);
```

注意：`Where`、`Select` 是延迟执行，调用 `ToList()` 才立即生成新集合。

## 6. 字符串处理

### 6.1 常用方法

| 方法                        | 作用            | 示例                                 |
| ------------------------- | ------------- | ---------------------------------- |
| `Length`                  | 长度            | `"abc".Length` → 3                 |
| `ToUpper()` / `ToLower()` | 大小写转换         | `"abc".ToUpper()` → "ABC"          |
| `Trim()`                  | 去两端空格         | `"  a ".Trim()` → "a"              |
| `Substring(起点, 长度)`       | 截取子串          | `"abcdef".Substring(1, 3)` → "bcd" |
| `Replace(旧, 新)`           | 替换            | `"aab".Replace("a", "x")` → "xxb"  |
| `Contains(s)`             | 是否包含          | `"abc".Contains("b")` → true       |
| `IndexOf(s)`              | 子串位置，未找到返回 -1 | `"abc".IndexOf("c")` → 2           |
| `Split(分隔符)`              | 拆分为数组         | `"a,b,c".Split(',')`               |
| `string.Join(分隔符, 集合)`    | 拼接为字符串        | `string.Join(",", arr)`            |
| `string.IsNullOrEmpty(s)` | 是否为 null 或空串  | 输入校验常用                             |

注意：C# 字符串下标从 0 开始（SQL 的 `substring` 从 1 开始）。

### 6.2 字符串格式化

```csharp
// string.Format：占位符从 0 编号
// 示例：StudentService.cs 中拼接 SQL
string s1 = string.Format("姓名：{0}，年龄：{1}", name, age);

// $ 字符串插值（推荐）：直接嵌入变量，与前端模板字符串一致
string s2 = $"姓名：{name}，年龄：{age}";
```

### 6.3 StringBuilder

`string` 不可变，循环内频繁拼接会产生大量临时对象，应改用 `StringBuilder`：

```csharp
// 示例：StudentService.cs 的 AddStudent 方法拼接 SQL
StringBuilder sb = new StringBuilder();
sb.Append("insert into Students ");
sb.Append("values(...)");
string sql = sb.ToString();   // 拼完一次性转成 string
```

## 7. 面向对象

面向对象的核心：把相关的数据和操作封装成类（模板），用类 new 出对象（实例）来完成工作。三大特性为封装、继承、多态。

### 7.1 类与对象

```csharp
// 示例：Student.cs 学员实体类
public class Student
{
    // 属性：对象的数据
    public int StudentId { get; set; }
    public string StudentName { get; set; }
    public int Age { get; set; }

    // 方法：对象的行为
    public string GetInfo()
    {
        return $"姓名：{StudentName}，年龄：{Age}";
    }
}

// 使用：new 创建对象 → 属性赋值 → 调用方法
Student stu = new Student();
stu.StudentName = "张三";

// 对象初始化器：new 的同时赋值，等价于上面两步
// 示例：StudentService.cs 中 new Student() { ... }
Student stu2 = new Student()
{
    StudentName = "李四",
    Age = 22
};
```

### 7.2 属性（Property）

属性封装字段，解决"字段直接暴露、无法拦截非法赋值"的问题。

```csharp
// 自动属性：编译器自动生成背后字段
public string StudentName { get; set; }

// 完整写法：私有字段 + 公开属性，set 中可加校验
private int age;
public int Age
{
    get { return age; }
    set
    {
        if (value < 0 || value > 150)
        {
            throw new Exception("年龄不合法");
        }
        age = value;
    }
}

// 受限访问：外部只读，仅类内部可写
public int StudentId { get; private set; }
```

### 7.3 方法（Method）

结构为 `返回值类型 方法名(参数列表) { 方法体 }`。

```csharp
// void 表示无返回值
public void Show() { }

// 方法重载（Overload）：同名方法、参数列表不同，编译器按实参自动匹配
// 示例：StudentService.cs 中两个 IsIdNoExistd：
// 新增学员时只查身份证；修改学员时还要排除自己
public bool IsIdNoExistd(string idNo) { }
public bool IsIdNoExistd(string idNo, int studentId) { }

// 可选参数：带默认值的参数可省略，必须排在参数列表末尾
public void Search(string name = "", int age = 0) { }
```

### 7.4 参数修饰符（ref / out / params）

- `ref`：按引用传参，方法内修改反映到调用方，传参前必须赋值。
- `out`：按引用传参，用于带回多个结果，传参前不必赋值，方法返回前必须赋值。
- `params`：可变参数，传任意个数同类型实参，方法内按数组处理。

```csharp
// ref：交换两个变量（值类型默认按值传递，不加 ref 改了外面不变）
public void Swap(ref int a, ref int b)
{
    int temp = a; a = b; b = temp;
}
int x = 1, y = 2;
Swap(ref x, ref y);       // 调用处也加 ref，执行后 x = 2，y = 1

// out：同时带回"是否成功"和"结果"
public bool TryDivide(int a, int b, out int result)
{
    if (b == 0) { result = 0; return false; }
    result = a / b;
    return true;
}
TryDivide(10, 3, out int r);

// params：任意个数参数
public int Sum(params int[] numbers) { }
Sum(1, 2, 3, 4);
```

`int.TryParse("123", out int f)` 正是用 `out` 实现：返回值表示成功与否，结果通过 `out` 带出。

### 7.5 构造函数（Constructor）

`new` 对象时自动执行，用于初始化。规则：与类同名、无返回值类型、可重载；自己不写则编译器送一个无参的，一旦写了带参的，默认无参版不再提供。

```csharp
public class Student
{
    public Student() { }                   // 无参

    public Student(string name, int age)   // 带参：创建时直接完成赋值
    {
        StudentName = name;
        Age = age;
    }
}

Student stu = new Student("张三", 20);
```

### 7.6 静态成员（static）

`static` 成员属于类而非对象，全程序共享一份，通过类名直接访问。

```csharp
// 示例：SQLHelper.cs 通用数据访问类
public class SQLHelper
{
    public static string connString = "...";   // 全程序共用同一连接字符串

    public static int Update(string sql) { }   // 类名直接调用，无需 new
}

SQLHelper.Update(sql);
```

| 对比项  | 实例成员            | 静态成员        |
| ---- | --------------- | ----------- |
| 归属   | 每个对象各一份         | 全类共享一份      |
| 调用方式 | `对象.成员`         | `类名.成员`     |
| 适用场景 | 每个对象各自的数据（学生姓名） | 工具方法、全局共享数据 |

注意：静态方法中不能直接访问实例成员，因为执行时不存在任何具体对象。

### 7.7 访问修饰符

| 修饰符         | 可访问范围           |
| ----------- | --------------- |
| `public`    | 任何地方            |
| `private`   | 仅当前类内部（类成员默认级别） |
| `protected` | 当前类及子类          |
| `internal`  | 同一程序集（项目）内      |

常用约定：字段一律 `private`，对外通过 `public` 属性暴露；只给子类用的用 `protected`。

### 7.8 继承（Inheritance）

把多个类的共性写在父类中，子类自动拥有，只写特有成员。C# 只支持单继承。

```csharp
// 父类：共性成员
public class Person
{
    public string Name { get; set; }
    public void Eat() { }
}

// 子类：冒号继承，只写特有成员
public class Student : Person
{
    public int StudentId { get; set; }
}

Student stu = new Student();
stu.Name = "张三";   // 继承自 Person
```

`base` 关键字用于在子类中访问父类成员：

```csharp
public Student(string name) : base(name) { }   // 调用父类构造函数
```

注意：`sealed` 修饰的类不能被继承，`string` 就是典型的密封类。

### 7.9 多态（virtual / override）

同一个方法调用，根据对象的实际类型执行不同版本。分三步：

```csharp
// 第一步：父类方法加 virtual，表示允许子类重写
public class Person
{
    public virtual void SayHello()
    {
        Console.WriteLine("我是人");
    }
}

// 第二步：子类加 override 重写
public class Student : Person
{
    public override void SayHello()
    {
        Console.WriteLine("我是学生");
    }
}

// 第三步：父类引用指向子类对象——多态生效的关键
Person p = new Student();
p.SayHello();   // 输出"我是学生"，由实际对象类型决定，而非引用类型
```

价值：写接收父类参数的通用方法，传入任何子类都自动执行对应实现，新增子类不用改通用方法。

### 7.10 抽象类（abstract）

不能被实例化，专作公共基类。`abstract` 方法没有方法体，子类必须重写。

```csharp
public abstract class Animal
{
    public void Eat() { }                 // 普通方法：含实现，子类直接复用
    public abstract void MakeSound();     // 抽象方法：只有签名，强制子类实现
}

public class Dog : Animal
{
    public override void MakeSound()      // 不实现则编译报错
    {
        Console.WriteLine("汪汪");
    }
}

// Animal a = new Animal();   // 错误：抽象类不能实例化
Animal dog = new Dog();       // 正确：父类引用指向子类对象
```

### 7.11 类型判断（is / as）

- `is`：判断对象是否为指定类型（或其子类），返回 `bool`。
- `as`：尝试转换，失败返回 `null`，不抛异常。

```csharp
Person p = new Student();

if (p is Student)
{
    Student s = (Student)p;       // 确认类型后再强转，安全
}

Student s2 = p as Student;        // 失败时 s2 为 null

// C# 7+ 模式匹配：判断与转换一步完成
if (p is Student s3) { }
```

### 7.12 接口（Interface）

接口只定义签名不包含实现，作用是"立规范"。使用分三步：定义接口 → 类继承接口 → 实现全部成员（少一个编译报错）。

```csharp
// 第一步：定义接口，命名习惯以 I 开头
public interface IStudentService
{
    int AddStudent(Student stu);
    List<Student> GetStudents();
}

// 第二、三步：继承并实现全部成员
public class StudentService : IStudentService
{
    public int AddStudent(Student stu) { }
    public List<Student> GetStudents() { }
}
```

价值：调用方只依赖接口不依赖具体类，换实现类时调用方不用改。

接口与抽象类的区别：

| 对比项  | 接口     | 抽象类       |
| ---- | ------ | --------- |
| 方法实现 | 不能包含实现 | 可包含已实现的方法 |
| 继承数量 | 可实现多个  | 只能继承一个    |
| 字段   | 不能定义   | 可以定义      |

## 8. 委托与事件

### 8.1 委托（Delegate）

委托是方法的类型化引用：把方法像变量一样存储、传递、调用，相当于前端的回调函数类型。典型场景：添加窗体新增学员后要通知管理窗体刷新列表，两个窗体不应互相 `new`，通过委托解耦。

委托的基本使用分三步（为便于演示写在一起，实际开发中三步常分布在不同文件中，跨文件用法见 8.2）：

```csharp
// 第一步：定义委托类型——约定能引用的方法长什么样（参数、返回值）
public delegate void DataHandler(string data);

// 签名匹配的方法
public void ReceiveData(string data)
{
    Console.WriteLine("收到: " + data);
}

// 第二步：委托变量指向方法（方法名后不加括号，加括号是调用不是取引用）
DataHandler handler = ReceiveData;

// 第三步：调用委托，等同于调用 ReceiveData("张三")
handler("张三");
```

**多播委托**：一个委托可挂多个方法，`+=` 挂载、`-=` 卸载，调用时按顺序依次执行。

```csharp
DataHandler handler = MethodA;
handler += MethodB;       // 挂两个方法
handler("张三");           // 依次执行 MethodA、MethodB
handler -= MethodA;       // 卸载
```

注意：没有挂载任何方法时委托为 `null`，直接调用抛 `NullReferenceException`，调用前先判空。

### 8.2 事件（Event）

裸委托字段是 `public` 的，外部可 `=` 覆盖、可随意触发，权力过大。事件用 `event` 把委托包一层：外部只能 `+=` / `-=`，触发权收回类内部。

事件机制涉及多个文件协作，下面按文件分布演示完整使用过程（四步）。

第一步，在公共位置定义委托类型（也可以直接定义在发布者文件中）：

```csharp
// ── Program.cs
public delegate void DataHandler(string data);
```

第二、三步，在发布者中声明事件并封装触发方法：

```csharp
// ── FrmAddStudent.cs（发布者）
public class FrmAddStudent
{
    // 第二步：event 关键字声明事件
    public static event DataHandler StudentChanged;

    // 第三步：类内部封装触发方法，外部想引发事件只能调它
    public static void NotifyStudentChanged(string name)
    {
        if (StudentChanged != null)   // 触发前必须判空
        {
            StudentChanged(name);
        }
    }

    // 添加学员按钮：保存成功后引发事件
    private void btnAdd_Click(object sender, EventArgs e)
    {
        // ...保存学员到数据库...
        NotifyStudentChanged(studentName);
    }
}
```

第四步，在订阅者中挂载处理方法：

```csharp
// ── FrmStudentManage.cs（订阅者）
FrmAddStudent.StudentChanged += OnStudentChanged;   // 窗体加载时订阅

private void OnStudentChanged(string name)
{
    // 刷新学员列表
}

// 窗体关闭时必须 -=：静态事件生命周期长于窗体，
// 不取消会一直持有窗体引用，造成内存泄漏
FrmAddStudent.StudentChanged -= OnStudentChanged;
```

委托与事件的区别：

| 对比项            | 委托（裸字段）   | 事件（event） |
| -------------- | --------- | --------- |
| 外部 `=` 赋值      | 允许，覆盖已有挂载 | 不允许       |
| 外部直接触发         | 允许        | 仅所属类内部    |
| 外部 `+=` / `-=` | 允许        | 允许        |

一句话：事件是限制了外部权限的委托。

### 8.3 Lambda 表达式

匿名方法的简写，适合只用一次的方法。`=>` 左边参数，右边方法体。

```csharp
// 匿名方法
list.Find(delegate(Student s) { return s.Age > 20; });

// Lambda：类型可推断故省略，一行时可省大括号和 return
list.Find(s => s.Age > 20);

// 订阅事件时内联处理逻辑
btnSend.Click += (sender, e) => { MessageBox.Show("点击了"); };
```

## 9. 异常处理

### 9.1 try-catch-finally

```csharp
try
{
    // 可能出错的代码
    int result = Convert.ToInt32(input);
}
catch (FormatException ex)
{
    // 捕获特定异常，ex.Message 是错误描述
    MessageBox.Show($"格式错误：{ex.Message}");
}
catch (Exception ex)
{
    // 所有异常的父类，兜底，必须放最后
    MessageBox.Show($"操作失败：{ex.Message}");
}
finally
{
    // 无论是否出错都执行，用于释放资源
    conn.Close();
}
```

执行流程：`try` 中出错 → 跳到匹配的 `catch` → 执行 `finally`，程序不崩溃；没出错则跳过 `catch` 直接执行 `finally`。

常见异常类型：

| 异常类型                       | 典型触发场景           |
| -------------------------- | ---------------- |
| `NullReferenceException`   | 调用了 null 对象的成员   |
| `FormatException`          | 字符串转数值格式错误       |
| `IndexOutOfRangeException` | 数组或集合下标越界        |
| `SqlException`             | 数据库连接失败、SQL 语法错误 |

### 9.2 throw 抛出异常

捕获后包装再抛出，让上层获得更明确的错误信息：

```csharp
// 示例：StudentService.cs 中数据访问层抛给界面层统一提示
catch (Exception ex)
{
    throw new Exception("添加学员失败，数据库操作异常：" + ex.Message);
}
```

## 10. 资源释放（using 语句）

实现 `IDisposable` 的资源（数据库连接、文件流）用完必须释放。`using` 确保离开作用域时自动释放，等价于 try-finally 中调 `Dispose()`。

```csharp
using (SqlConnection conn = new SqlConnection(connString))
{
    conn.Open();
    // 使用连接
}   // 自动调用 conn.Dispose()，中途出错也会执行
```

## 11. 常用类库

### 11.1 DateTime 常用操作

| 成员                                         | 作用            |
| ------------------------------------------ | ------------- |
| `DateTime.Now`                             | 当前日期时间        |
| `DateTime.Today`                           | 当前日期（时间部分为 0） |
| `Year` / `Month` / `Day`                   | 取年、月、日        |
| `AddDays(n)` / `AddYears(n)`               | 日期加减          |
| `ToString("格式")`                           | 格式化输出         |
| `DateTime.Parse(s)` / `TryParse(s, out d)` | 字符串转日期        |

```csharp
DateTime now = DateTime.Now;
string s1 = now.ToString("yyyy-MM-dd HH:mm:ss"); // 2026-10-17 18:30:00
DateTime weekLater = now.AddDays(7);

// 计算周岁
int age = DateTime.Now.Year - birthday.Year;
```

注意：`DateTime` 与 SQL Server 的 `datetime` 对应，写入数据库时直接以 `DateTime` 对象作参数传递即可。

### 11.2 文件读写（File）

`System.IO` 命名空间下的 `File` 类提供操作文件的静态方法。

```csharp
using System.IO;

File.WriteAllText("log.txt", "内容");      // 写入（覆盖）
File.AppendAllText("log.txt", "追加");     // 追加
string content = File.ReadAllText("log.txt");   // 读取全部
string[] lines = File.ReadAllLines("log.txt");  // 按行读取
bool exists = File.Exists("log.txt");           // 判断是否存在
```

