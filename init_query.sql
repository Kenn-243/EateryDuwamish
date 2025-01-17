USE [EateryDB]
GO
/****** Object:  UserDefinedFunction [dbo].[fn_General_Split]    Script Date: 20/05/2021 7:23:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[fn_General_Split]
(
	@list VARCHAR(MAX),
	@delimiter VARCHAR(5)
)
RETURNS @retVal TABLE (Id INT IDENTITY(1,1), Value VARCHAR(MAX))
AS
BEGIN
	WHILE (CHARINDEX(@delimiter, @list) > 0)
	BEGIN
		INSERT INTO @retVal (Value)
		SELECT Value = LTRIM(RTRIM(SUBSTRING(@list, 1, CHARINDEX(@delimiter, @list) - 1)))
		SET @list = SUBSTRING(@list, CHARINDEX(@delimiter, @list) + LEN(@delimiter), LEN(@list))
	END
	INSERT INTO @retVal (Value)
	SELECT Value = LTRIM(RTRIM(@list))
	RETURN 
END

GO
/****** Object:  Table [dbo].[msDish]    Script Date: 20/05/2021 7:23:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[msDish](
	[DishID] [int] IDENTITY(1,1) NOT NULL,
	[DishTypeID] [int] NOT NULL,
	[DishName] [varchar](200) NOT NULL,
	[DishPrice] [int] NOT NULL,
	[AuditedActivity] [char](1) NOT NULL,
	[AuditedTime] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[DishID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[msDishType]    Script Date: 20/05/2021 7:23:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[msDishType](
	[DishTypeID] [int] IDENTITY(1,1) NOT NULL,
	[DishTypeName] [varchar](100) NOT NULL,
	[AuditedActivity] [char](1) NOT NULL,
	[AuditedTime] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[DishTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[msDish]  WITH CHECK ADD FOREIGN KEY([DishTypeID])
REFERENCES [dbo].[msDishType] ([DishTypeID])

GO
/****** Object:  StoredProcedure [dbo].[Dish_Delete]    Script Date: 20/05/2021 7:23:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/**
 * Created by: Jonathan Ibrahim
 * Date: 10 Mar 2021
 * Purpose: Delete dish
 */
CREATE PROCEDURE [dbo].[Dish_Delete]
	@DishIDs VARCHAR(MAX)
AS
BEGIN
	UPDATE msDish
	SET AuditedActivity = 'D',
		AuditedTime = GETDATE()
	WHERE DishID IN (SELECT value FROM fn_General_Split(@DishIDs, ','))
END

GO
/****** Object:  StoredProcedure [dbo].[Dish_Get]    Script Date: 20/05/2021 7:23:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/**
 * Created by: Jonathan Ibrahim
 * Date: 10 Mar 2021
 * Purpose: Get semua dish
 */
CREATE PROCEDURE [dbo].[Dish_Get]
AS
BEGIN
	SELECT 
		DishID,
		DishTypeID,
		DishName, 
		DishPrice 
	FROM msDish WITH(NOLOCK)
	WHERE AuditedActivity <> 'D'
END

GO
/****** Object:  StoredProcedure [dbo].[Dish_GetByID]    Script Date: 20/05/2021 7:23:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/**
 * Created by: Jonathan Ibrahim
 * Date: 10 Mar 2021
 * Purpose: Get dish tertentu by Id
 */
CREATE PROCEDURE [dbo].[Dish_GetByID]
	@DishId INT
AS
BEGIN
	SELECT 
		DishID,
		DishTypeID,
		DishName, 
		DishPrice 
	FROM msDish WITH(NOLOCK)
	WHERE DishId = @DishId AND AuditedActivity <> 'D'
END

GO
/****** Object:  StoredProcedure [dbo].[Dish_InsertUpdate]    Script Date: 20/05/2021 7:23:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/**
 * Created by: Jonathan Ibrahim
 * Date: 10 Mar 2021
 * Purpose: Insert atau update dish
 */
CREATE PROCEDURE [dbo].[Dish_InsertUpdate]
	@DishID INT OUTPUT,
	@DishTypeID INT,
	@DishName VARCHAR(100),
	@DishPrice INT
AS
BEGIN
	DECLARE @RetVal INT
	IF EXISTS (SELECT 1 FROM msDish WITH(NOLOCK) WHERE DishID = @DishID AND AuditedActivity <> 'D')
	BEGIN
		UPDATE msDish
		SET DishName = @DishName,
			DishTypeID = @DishTypeID,
			DishPrice = @DishPrice,
			AuditedActivity = 'U',
			AuditedTime = GETDATE()
		WHERE DishID = @DishID AND AuditedActivity <> 'D'
		SET @RetVal = @DishID
	END
	ELSE
	BEGIN
		INSERT INTO msDish 
		(DishName, DishTypeID, DishPrice, AuditedActivity, AuditedTime)
		VALUES
		(@DishName, @DishTypeID, @DishPrice, 'I', GETDATE())
		SET @RetVal = SCOPE_IDENTITY()
	END
	SELECT @DishId = @RetVal
END

GO
/****** Object:  StoredProcedure [dbo].[DishType_Get]    Script Date: 20/05/2021 7:23:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/**
 * Created by: Jonathan Ibrahim
 * Date: 10 Mar 2021
 * Purpose: Get semua dish type
 */
CREATE PROCEDURE [dbo].[DishType_Get]
AS
BEGIN
	SELECT DishTypeID, DishTypeName FROM msDishType WITH(NOLOCK) 
	WHERE AuditedActivity <> 'D'
END

GO
/****** Object:  StoredProcedure [dbo].[DishType_GetByID]    Script Date: 20/05/2021 7:23:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/**
 * Created by: Jonathan Ibrahim
 * Date: 10 Mar 2021
 * Purpose: Get dish type by ID
 */
CREATE PROCEDURE [dbo].[DishType_GetByID]
	@DishTypeID INT
AS
BEGIN
	SELECT DishTypeID, DishTypeName
	FROM msDishType WITH(NOLOCK)
	WHERE DishTypeID = @DishTypeID AND AuditedActivity <> 'D'
END
GO
-- SEEDING msDishType
INSERT INTO msDishType (DishTypeName,AuditedActivity,AuditedTime)
VALUES ('Rumahan','I',GETDATE()), ('Restoran','I',GETDATE()), ('Pinggiran','I',GETDATE())


/* EDIT */



GO
/****** Object:  Table [dbo].[msRecipe]    Script Date: 20/05/2021 7:23:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[msRecipe](
	[RecipeID] [int] IDENTITY(1,1) NOT NULL,
	[DishID] [int] NOT NULL,
	[RecipeName] [varchar](50) NOT NULL,
	[AuditedActivity] [char](1) NOT NULL,
	[AuditedTime] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[RecipeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
ALTER TABLE [dbo].[msRecipe]  WITH CHECK ADD FOREIGN KEY([DishID])
REFERENCES [dbo].[msDish] ([DishID])

GO
/****** Object:  StoredProcedure [dbo].[Recipe_Delete]    Script Date: 20/05/2021 7:23:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/**
 * Created by: Jonathan Ibrahim
 * Date: 10 Mar 2021
 * Purpose: Delete dish
 */
CREATE PROCEDURE [dbo].[Recipe_Delete]
	@RecipeIDs VARCHAR(MAX)
AS
BEGIN
	UPDATE msRecipe
	SET AuditedActivity = 'D',
		AuditedTime = GETDATE()
	WHERE RecipeID IN (SELECT value FROM fn_General_Split(@RecipeIDs, ','))
END

GO
/****** Object:  StoredProcedure [dbo].[Recipe_Get]    Script Date: 20/05/2021 7:23:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/**
 * Created by: Jonathan Ibrahim
 * Date: 10 Mar 2021
 * Purpose: Get semua dish
 */
CREATE PROCEDURE [dbo].[Recipe_Get]
AS
BEGIN
	SELECT 
		RecipeID,
		DishID,
		RecipeName 
	FROM msRecipe WITH(NOLOCK)
	WHERE AuditedActivity <> 'D'
END

GO
/****** Object:  StoredProcedure [dbo].[Recipe_GetByID]    Script Date: 20/05/2021 7:23:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/**
 * Created by: Jonathan Ibrahim
 * Date: 10 Mar 2021
 * Purpose: Get dish tertentu by Id
 */
CREATE PROCEDURE [dbo].[Recipe_GetByID]
	@RecipeId INT
AS
BEGIN
	SELECT 
		RecipeID,
		DishID,
		RecipeName
	FROM msRecipe WITH(NOLOCK)
	WHERE RecipeId = @RecipeId AND AuditedActivity <> 'D'
END

GO
/****** Object:  StoredProcedure [dbo].[Recipe_InsertUpdate]    Script Date: 20/05/2021 7:23:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/**
 * Created by: Jonathan Ibrahim
 * Date: 10 Mar 2021
 * Purpose: Insert atau update dish
 */
CREATE PROCEDURE [dbo].[Recipe_InsertUpdate]
	@RecipeID INT OUTPUT,
	@DishID INT,
	@RecipeName VARCHAR(50)
AS
BEGIN
	DECLARE @RetVal INT
	IF EXISTS (SELECT 1 FROM msRecipe WITH(NOLOCK) WHERE RecipeID = @RecipeID AND AuditedActivity <> 'D')
	BEGIN
		UPDATE msRecipe
		SET RecipeName = @RecipeName,
			DishID = @DishID,
			AuditedActivity = 'U',
			AuditedTime = GETDATE()
		WHERE RecipeID = @RecipeID AND AuditedActivity <> 'D'
		SET @RetVal = @RecipeID
	END
	ELSE
	BEGIN
		INSERT INTO msRecipe 
		(DishID, RecipeName, AuditedActivity, AuditedTime)
		VALUES
		(@DishID, @RecipeName, 'I', GETDATE())
		SET @RetVal = SCOPE_IDENTITY()
	END
	SELECT @RecipeId = @RetVal
END

GO
/****** Object:  Table [dbo].[msDetail]    Script Date: 20/05/2021 7:23:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[msDetail](
	[DetailID] [int] IDENTITY(1,1) NOT NULL,
	[RecipeID] [int] NOT NULL,
	[DetailIngredient] [varchar](50) NOT NULL,
	[DetailQuantity] [int] NOT NULL,
	[DetailUnit] [varchar](10) NOT NULL,
	[AuditedActivity] [char](1) NOT NULL,
	[AuditedTime] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[DetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
ALTER TABLE [dbo].[msDetail]  WITH CHECK ADD FOREIGN KEY([RecipeID])
REFERENCES [dbo].[msRecipe] ([RecipeID])

GO
/****** Object:  StoredProcedure [dbo].[Detail_Delete]    Script Date: 20/05/2021 7:23:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/**
 * Created by: Jonathan Ibrahim
 * Date: 10 Mar 2021
 * Purpose: Delete dish
 */
CREATE PROCEDURE [dbo].[Detail_Delete]
	@DetailIDs VARCHAR(MAX)
AS
BEGIN
	UPDATE msDetail
	SET AuditedActivity = 'D',
		AuditedTime = GETDATE()
	WHERE DetailID IN (SELECT value FROM fn_General_Split(@DetailIDs, ','))
END

GO
/****** Object:  StoredProcedure [dbo].[Detail_Get]    Script Date: 20/05/2021 7:23:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/**
 * Created by: Jonathan Ibrahim
 * Date: 10 Mar 2021
 * Purpose: Get semua dish
 */
CREATE PROCEDURE [dbo].[Detail_Get]
AS
BEGIN
	SELECT 
		DetailID,
		RecipeID,
		DetailIngredient,
		DetailQuantity,
		DetailUnit
	FROM msDetail WITH(NOLOCK)
	WHERE AuditedActivity <> 'D'
END

GO
/****** Object:  StoredProcedure [dbo].[Detail_GetByID]    Script Date: 20/05/2021 7:23:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/**
 * Created by: Jonathan Ibrahim
 * Date: 10 Mar 2021
 * Purpose: Get dish tertentu by Id
 */
CREATE PROCEDURE [dbo].[Detail_GetByID]
	@DetailId INT
AS
BEGIN
	SELECT 
		DetailID,
		RecipeID,
		DetailIngredient,
		DetailQuantity,
		DetailUnit
	FROM msDetail WITH(NOLOCK)
	WHERE DetailId = @DetailId AND AuditedActivity <> 'D'
END

GO
/****** Object:  StoredProcedure [dbo].[Detail_InsertUpdate]    Script Date: 20/05/2021 7:23:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/**
 * Created by: Jonathan Ibrahim
 * Date: 10 Mar 2021
 * Purpose: Insert atau update dish
 */
CREATE PROCEDURE [dbo].[Detail_InsertUpdate]
	@DetailID INT OUTPUT,
	@RecipeID INT,
	@DetailIngredient VARCHAR(50),
	@DetailQuantity INT,
	@DetailUnit VARCHAR(10)
AS
BEGIN
	DECLARE @RetVal INT
	IF EXISTS (SELECT 1 FROM msDetail WITH(NOLOCK) WHERE DetailID = @DetailID AND AuditedActivity <> 'D')
	BEGIN
		UPDATE msDetail
		SET RecipeID = @RecipeID,
			DetailIngredient = @DetailIngredient,
			DetailQuantity = @DetailQuantity,
			DetailUnit = @DetailUnit,
			AuditedActivity = 'U',
			AuditedTime = GETDATE()
		WHERE DetailID = @DetailID AND AuditedActivity <> 'D'
		SET @RetVal = @DetailID
	END
	ELSE
	BEGIN
		INSERT INTO msDetail
		(RecipeID, DetailIngredient, DetailQuantity, DetailUnit, AuditedActivity, AuditedTime)
		VALUES
		(@RecipeID, @DetailIngredient, @DetailQuantity, @DetailUnit, 'I', GETDATE())
		SET @RetVal = SCOPE_IDENTITY()
	END
	SELECT @DetailId = @RetVal
END

GO
/****** Object:  Table [dbo].[msDescription]    Script Date: 20/05/2021 7:23:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[msDescription](
	[DescriptionID] [int] IDENTITY(1,1) NOT NULL,
	[RecipeID] [int] NOT NULL,
	[DescriptionRecipe] [varchar](MAX) NOT NULL,
	[AuditedActivity] [char](1) NOT NULL,
	[AuditedTime] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[DescriptionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
ALTER TABLE [dbo].[msDescription]  WITH CHECK ADD FOREIGN KEY([RecipeID])
REFERENCES [dbo].[msRecipe] ([RecipeID])

GO
/****** Object:  StoredProcedure [dbo].[Description_Delete]    Script Date: 20/05/2021 7:23:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/**
 * Created by: Jonathan Ibrahim
 * Date: 10 Mar 2021
 * Purpose: Delete dish
 */
CREATE PROCEDURE [dbo].[Description_Delete]
	@DescriptionIDs VARCHAR(MAX)
AS
BEGIN
	UPDATE msDescription
	SET AuditedActivity = 'D',
		AuditedTime = GETDATE()
	WHERE DescriptionID IN (SELECT value FROM fn_General_Split(@DescriptionIDs, ','))
END

GO
/****** Object:  StoredProcedure [dbo].[Description_Get]    Script Date: 20/05/2021 7:23:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/**
 * Created by: Jonathan Ibrahim
 * Date: 10 Mar 2021
 * Purpose: Get semua dish
 */
CREATE PROCEDURE [dbo].[Description_Get]
AS
BEGIN
	SELECT 
		DescriptionID,
		RecipeID,
		DescriptionRecipe
	FROM msDescription WITH(NOLOCK)
	WHERE AuditedActivity <> 'D'
END

GO
/****** Object:  StoredProcedure [dbo].[Description_GetByID]    Script Date: 20/05/2021 7:23:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/**
 * Created by: Jonathan Ibrahim
 * Date: 10 Mar 2021
 * Purpose: Get dish tertentu by Id
 */
CREATE PROCEDURE [dbo].[Description_GetByID]
	@DescriptionId INT
AS
BEGIN
	SELECT 
		DescriptionID,
		RecipeID,
		DescriptionRecipe
	FROM msDescription WITH(NOLOCK)
	WHERE DescriptionId = @DescriptionId AND AuditedActivity <> 'D'
END

GO
/****** Object:  StoredProcedure [dbo].[Description_InsertUpdate]    Script Date: 20/05/2021 7:23:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/**
 * Created by: Jonathan Ibrahim
 * Date: 10 Mar 2021
 * Purpose: Insert atau update dish
 */
CREATE PROCEDURE [dbo].[Description_InsertUpdate]
	@DescriptionID INT OUTPUT,
	@RecipeID INT,
	@DescriptionRecipe VARCHAR(MAX)
AS
BEGIN
	DECLARE @RetVal INT
	IF EXISTS (SELECT 1 FROM msDescription WITH(NOLOCK) WHERE DescriptionID = @DescriptionID AND AuditedActivity <> 'D')
	BEGIN
		UPDATE msDescription
		SET RecipeID = @RecipeID,
			DescriptionRecipe = @DescriptionRecipe,
			AuditedActivity = 'U',
			AuditedTime = GETDATE()
		WHERE DescriptionID = @DescriptionID AND AuditedActivity <> 'D'
		SET @RetVal = @DescriptionID
	END
	ELSE
	BEGIN
		INSERT INTO msDescription
		(RecipeID, DescriptionRecipe, AuditedActivity, AuditedTime)
		VALUES
		(@RecipeID, @DescriptionRecipe, 'I', GETDATE())
		SET @RetVal = SCOPE_IDENTITY()
	END
	SELECT @DescriptionId = @RetVal
END