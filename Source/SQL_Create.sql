 drop table Payment; 
 drop Table OrderDetail;
 drop Table Orders;
 drop Table CartDetail;
 drop Table ShoppingCart;
 drop table UserAccount;
 drop table Book;

CREATE TABLE [dbo].[UserAccount] (
    [userAccountID] INT            IDENTITY (10, 1) NOT NULL, 
    [password]      NVARCHAR (50)  NOT NULL,
    [firstname]     NVARCHAR (50)  NOT NULL,
    [lastname]      NVARCHAR (50)  NOT NULL,
    [address]       NVARCHAR (50)  NOT NULL,
    [city]          NVARCHAR (50)  NOT NULL,
    [postalcode]    NVARCHAR (50)  NOT NULL,
    [phone]         NVARCHAR (50)  NOT NULL,
    [email]         NVARCHAR (300) NOT NULL,
    PRIMARY KEY CLUSTERED ([userAccountID] ASC)
);

CREATE TABLE [dbo].[Book] (
    [bookID]       INT            IDENTITY (10, 1) NOT NULL,
    [category]     NVARCHAR (50)  NOT NULL,
    [title]        NVARCHAR (50)  NOT NULL,
    [author]       NVARCHAR (50)  NOT NULL,
    [description]  TEXT           NULL,
    [year]         INT            NULL,
    [isbn]         NVARCHAR (50)  NOT NULL,
    [publisher]    NVARCHAR (50)  NULL,
    [shippingCost] DECIMAL (7, 2) NULL,
    [price]        DECIMAL (7, 2) NULL,
    CONSTRAINT [PK_Book] PRIMARY KEY CLUSTERED ([bookID] ASC)
);

CREATE TABLE [dbo].[ShoppingCart] (
    [cartID]        INT IDENTITY (1, 1) NOT NULL,
    [userAccountID] INT NOT NULL,
    PRIMARY KEY CLUSTERED ([cartID] ASC),
    FOREIGN KEY ([userAccountID]) REFERENCES [dbo].[UserAccount] ([userAccountID])
);

CREATE TABLE [dbo].[CartDetail] (
    [cartDetailID] INT            IDENTITY (1, 1) NOT NULL,
    [bookID]       INT            NOT NULL,
    [cartID]       INT            NOT NULL,
    [quantity]     INT            NOT NULL,
    [price]        DECIMAL (7, 2) NOT NULL,
    [inDate]       DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([cartDetailID] ASC),
    FOREIGN KEY ([cartID]) REFERENCES [dbo].[ShoppingCart] ([cartID]),
    FOREIGN KEY ([bookID]) REFERENCES [dbo].[Book] ([bookID])
);

CREATE TABLE [dbo].[Orders] (
    [orderID]       INT            IDENTITY (1, 1) NOT NULL,
    [userAccountID] INT            NOT NULL,
    [orderStatus]   VARCHAR (20)   NULL,
    [inDate]        DATE           NOT NULL,
    [shippedDate]   DATE           NULL,
    [deliveredDate] DATE           NULL,
    [address]       VARCHAR (200)  NULL,
    [city]          VARCHAR (20)   NULL,
    [postalCode]    VARCHAR (7)    NULL,
    [phone]         VARCHAR (13)   NULL,
    [shippingCost]  DECIMAL (7, 2) NULL,
    [tax]           DECIMAL (4, 2) NULL,
    PRIMARY KEY CLUSTERED ([orderID] ASC),
    FOREIGN KEY ([userAccountID]) REFERENCES [dbo].[UserAccount] ([userAccountID])
);

CREATE TABLE [dbo].[OrderDetail] (
	[orderDetailID]  INT            IDENTITY (1, 1) NOT NULL PRIMARY KEY,
    [orderID]  INT            NOT NULL,
    [bookID]   INT            NOT NULL,
    [quantity] INT            NOT NULL,
    [price]    DECIMAL (7, 2) NOT NULL,
    FOREIGN KEY ([orderID]) REFERENCES [dbo].[Orders] ([orderID]),
    FOREIGN KEY ([bookID]) REFERENCES [dbo].[Book] ([bookID])
);



CREATE TABLE [dbo].[Payment] (
    [paymentID]     INT            IDENTITY (1, 1) NOT NULL,
    [orderID]       INT            NOT NULL,
    [payType]       VARCHAR (20)   NULL,
    [cardNumber]    CHAR (16)      NULL,
    [nameOnTheCard] VARCHAR (20)   NULL,
    [amount]        DECIMAL (7, 2) NULL,
    [inDate]        DATETIME       NOT NULL,
    PRIMARY KEY CLUSTERED ([paymentID] ASC),
    FOREIGN KEY ([orderID]) REFERENCES [dbo].[Orders] ([orderID])
);



/* Insert Book Data */
insert into book (category,title,author,description,year,isbn,publisher,price)
values (
'FICTION',
'Harry Potter A History Of Magic',
'J.K Rowling',
'Harry Potter: A History of Magic is the official book of the exhibition, a once-in-a-lifetime collaboration between Bloomsbury, J.K. 
Rowling and the brilliant curators of the British Library. It promises to take readers on a fascinating journey through the subjects studied at Hogwarts School of Witchcraft and Wizardry - from Alchemy and Potions classes through to Herbology and Care of Magical Creatures.
Each chapter showcases a treasure trove of artefacts from the British Library and other 
collections around the world, besides exclusive manuscripts, sketches and illustrations from the Harry Potter archive
',
'2018',
'9781408890769',
'Bloomsbury',
'22.99' 
);

/*
FICTION,
NON_FICTION,
HISTORY,
COMICS,
ART
*/

insert into book (category,title,author,description,year,isbn,publisher,price)
values (
'FICTION',
'Mr. Terupt Falls Again',
'Rob Buyea',
'Fifth-grade was full of unforgettable events for Mr. Terupt and his class at Snow Hill School. Seven students were particularly affected by Mr. Terupt. Now those seven students are back, and they have been granted the rare opportunity to send one more year with their beloved teacher before they graduate from elementary school. Peter''s parents expect him to attend private school after sixth-grade, but Peter has plans to stay right where he is.',
'2013',
'0307930467',
'Random House',
'19.99'
);

insert into book (category,title,author,description,year,isbn,publisher,price)
values (
'FICTION',
'Left Out',
'Tim Green',
'Dorch wants to be like everyone else. But his deafness and the way he talks have always felt like insurmountable obstacles. But now he finally sees his chance to fit in. Bigger and taller than any other seventh grader in his new school, Landon plans to use his size to his advantage and join the school’s football team. But the same speech problems and cochlear implants that help him hear continue to haunt him.
Just when it looks like Landon will be left out of football for good, an unlikely friend comes along. But in the end only Landon can fight his way off the bench and through a crowded field of bullies bent on seeing him forever left out.
',
'2017',
'0307930467',
'Random House',
'19.99'
);

insert into book (category,title,author,description,year,isbn,publisher,price)
values (
'HISTORY',
'A History Of CANADA in Ten Maps',
'Adam Shoalts',
'Tell our countrys story through rich narrative, recreations of daily life, folk tales and intriguing facts. Coupled with Alan Daniels evocative original paintings, as well as dozens of historical photographs, maps, paintings, documents and cartoons, The Story of Canada is as splendid to look at as it is fascinating to read. Includes new material to bring us to the 150th anniversary of Confederation.',
'2016',
'330467',
'Canada Maple',
'49.99'
);

insert into book (category,title,author,description,year,isbn,publisher,price)
values (
'HISTORY',
'A History Of KOREA',
'Kyung Moon Hwang',
'Presenting the richness of Korean civilization from early state formation to the jarring transformations resulting in two distinctive trajectories of modern development, this book introduces the country major historical events, patterns, and debates. Organised both chronologically and thematically, it explore recurring themes such as Korean identity, external influence, and family and gender. This lively narrative assumes no prior knowledge, inviting readers to appreciate both the distinctiveness and universality of Korean history, while integrating it into East Asian history more broadly.',
'2016',
'1137573562',
'Red Globe Press',
'38.37'
);

insert into book (category,title,author,description,year,isbn,publisher,price)
values (
'ART',
'Easy Origami',
'John Montroll',
'Here is a collection of 32 simple projects for novice origami hobbyists — clearly illustrated and with easy-to-follow instructions that even beginning papercrafters can follow with success. Subjects range from an ultra-simple hat, cup, and pinwheel to the more challenging (but still unintimidating) penguin, pelican, and piano.
Also included are the figures of a swan, lantern, cicada, pigeon, fox, rabbit, and other popular origami subjects. With the successful completion of these projects, origami hobbyists will be well on their way to mastering a fascinating art that is as old as the invention of paper itself.'
,'2000',
'9780486272986',
'Dover Publications',
'9.99'
);


insert into book (category,title,author,description,year,isbn,publisher,price)
values (
'COMIC',
'Dog Man #6: Brawl Of The Wild',
'Dav Pilkey',
'Dog Man and the Supa Buddies are a paw-erful force against evil, but has Petey, the World''s Most Evil Cat, finally outsmarted them? Petey has created another confounding contraption and is determined to destroy Dog Man once and for all. Can Dog Man''s nose for justice and Li will Petey''s heart of gold finally convince the conniving kitty to do good?',
'2018',
'1338236571',
'SCHOLASTIC INC',
'11.90'
);

insert into book (category,title,author,description,year,isbn,publisher,price)
values (
'NON_FICTION',
'Milk and Honey',
'Rupi Kaur',
'The book is divided into four chapters, and each chapter serves a different purpose. Deals with a different pain. Heals a different heartache. Milk and Honey takes readers through a journey of the most bitter moments in life and finds sweetness in them because there is sweetness everywhere if you are just willing to look.'
,'2015',
'9781449474256',
'Andrews McMeel Publishing',
'12.68'
);


select * from book;