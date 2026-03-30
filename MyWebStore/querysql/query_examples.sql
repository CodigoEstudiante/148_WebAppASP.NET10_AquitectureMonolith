create database DemoDB

use DemoDB

create table Sale(
SaleId int primary key identity,
CustomerName varchar(50),
SaleDate date default getdate(),
TotalAmount decimal(10,2)
)


create table SaleDetail(
SaleDetailId int primary key identity,
SaleId int references Sale(SaleId),
ProductName varchar(50),
Price decimal(10,2),
Quantity int,
SubTotal AS (Price * Quantity) PERSISTED
)

select * from Sale

select * from SaleDetail


create type SaleDetailType as table
(
ProductName varchar(50),
Price decimal(10,2),
Quantity int
)

create proc sp_createSale(
@CustomerName varchar(50),
@TotalAmount decimal(10,2),
@Details SaleDetailType READONLY
)
as
begin

	begin try
		begin transaction

		declare @SaleId int

		insert into	Sale(CustomerName,TotalAmount)
			 VALUES (@CustomerName,@TotalAmount)

		set @SaleId =  SCOPE_IDENTITY()

		insert into SaleDetail(SaleId, ProductName,Price, Quantity)
		select  @SaleId, ProductName, Price, Quantity
		from @Details

		commit transaction

	end try
	begin catch
	  ROLLBACK TRANSACTION
	end catch
end


create proc sp_editSale(
@SaleId int,
@CustomerName varchar(50),
@TotalAmount decimal(10,2),
@Details SaleDetailType READONLY
)
as
begin

	begin try
		begin transaction


		update Sale set
		CustomerName =@CustomerName ,
		TotalAmount =@TotalAmount
		where SaleId = @SaleId

		delete from SaleDetail where SaleId = @SaleId

		insert into SaleDetail(SaleId, ProductName,Price, Quantity)
		select  @SaleId, ProductName, Price, Quantity
		from @Details

		commit transaction

	end try
	begin catch
	  ROLLBACK TRANSACTION
	end catch
end

create proc sp_deleteSale(
@SaleId int
)
as
begin
	delete from SaleDetail where SaleId = @SaleId
	delete from Sale where SaleId = @SaleId
end

create proc sp_getSales
as
begin
	select SaleId,CustomerName,SaleDate,TotalAmount 
	from Sale 
end


create proc sp_getSale(
@SaleId int
)
as
begin
	select SaleId,CustomerName,SaleDate,TotalAmount 
	from Sale where SaleId = @SaleId

	select SaleDetailId, ProductName,Price,Quantity,SubTotal 
	from SaleDetail where SaleId = @SaleId
end
