USE [master]
GO
/****** Object:  Database [CalculatorHistory]    Script Date: 26/01/2025 00:01:51 ******/
CREATE DATABASE [CalculatorHistory]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'CalculatorHistory', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\CalculatorHistory.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'CalculatorHistory_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\CalculatorHistory_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [CalculatorHistory] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [CalculatorHistory].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [CalculatorHistory] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [CalculatorHistory] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [CalculatorHistory] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [CalculatorHistory] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [CalculatorHistory] SET ARITHABORT OFF 
GO
ALTER DATABASE [CalculatorHistory] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [CalculatorHistory] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [CalculatorHistory] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [CalculatorHistory] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [CalculatorHistory] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [CalculatorHistory] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [CalculatorHistory] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [CalculatorHistory] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [CalculatorHistory] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [CalculatorHistory] SET  DISABLE_BROKER 
GO
ALTER DATABASE [CalculatorHistory] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [CalculatorHistory] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [CalculatorHistory] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [CalculatorHistory] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [CalculatorHistory] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [CalculatorHistory] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [CalculatorHistory] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [CalculatorHistory] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [CalculatorHistory] SET  MULTI_USER 
GO
ALTER DATABASE [CalculatorHistory] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [CalculatorHistory] SET DB_CHAINING OFF 
GO
ALTER DATABASE [CalculatorHistory] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [CalculatorHistory] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [CalculatorHistory] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [CalculatorHistory] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [CalculatorHistory] SET QUERY_STORE = ON
GO
ALTER DATABASE [CalculatorHistory] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [CalculatorHistory]
GO
/****** Object:  Table [dbo].[Calculator]    Script Date: 26/01/2025 00:01:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Calculator](
	[InsertDate] [datetime] NULL,
	[Field1] [decimal](18, 2) NULL,
	[Field2] [decimal](18, 2) NULL,
	[Operation] [tinyint] NULL,
	[Result] [decimal](18, 2) NULL
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[GetHistory]    Script Date: 26/01/2025 00:01:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetHistory] 
	@Operation	tinyint

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT  TOP 3 * FROM Calculator
	WHERE Operation = @Operation
	ORDER BY InsertDate DESC;
		
END
GO
/****** Object:  StoredProcedure [dbo].[GetHistoryStats]    Script Date: 26/01/2025 00:01:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetHistoryStats] 
	@Operation	tinyint

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT  
		MAX(result) AS 'MaxResult',
		MIN(result) AS 'MinResult',
		AVG(result) AS 'AvgResult'
	From Calculator;
		
END
GO
/****** Object:  StoredProcedure [dbo].[GetLastMonthSearches]    Script Date: 26/01/2025 00:01:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetLastMonthSearches] 
	@Operation	tinyint

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT 
		COUNT(*) AS 'Count'
	FROM Calculator
	WHERE DATEDIFF(day, [InsertDate], CURRENT_TIMESTAMP) BETWEEN 0 AND 30
	AND [Operation] = @Operation;
		
END
GO
/****** Object:  StoredProcedure [dbo].[InsertHistory]    Script Date: 26/01/2025 00:01:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[InsertHistory] 
	-- Add the parameters for the stored procedure here
	@Date		Datetime,
	@Field1		Float,
	@Field2		Float,
	@Operation	tinyint,
	@Result		Float

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO Calculator (InsertDate, Field1, Field2, Operation, Result)
	VALUES (@Date, @Field1, @Field2, @Operation, @Result);
		
END
GO
USE [master]
GO
ALTER DATABASE [CalculatorHistory] SET  READ_WRITE 
GO
