CREATE TABLE Vendas (
    NumeroVenda INT IDENTITY(1,1) PRIMARY KEY,  
    DataVenda DATETIME NOT NULL,                
    Cliente NVARCHAR(100) NOT NULL,             
    ValorTotalVenda DECIMAL(18, 2) NOT NULL,    
    Filial NVARCHAR(50) NOT NULL,               
    Cancelado BIT NOT NULL                     
);
CREATE TABLE ItensVenda (
    Id INT IDENTITY(1,1) PRIMARY KEY,       
    NumeroVenda INT,                          
    Produto NVARCHAR(100) NOT NULL,          
    Quantidade INT NOT NULL,                
    ValorUnitario DECIMAL(18, 2) NOT NULL,   
    Desconto DECIMAL(18, 2) NOT NULL,        
    ValorTotalItem DECIMAL(18, 2) NOT NULL,  
    FOREIGN KEY (NumeroVenda) REFERENCES Vendas(NumeroVenda)  
);
