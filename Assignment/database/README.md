# SQL Test Assignment

**— Test Starts Here —**

1. Select users whose id is either 3,2 or 4
- Please return at least: all user fields
->  SELECT * 
	FROM users 
	where id in (2,3,4)

2. Count how many basic and premium listings each active user has
- Please return at least: first_name, last_name, basic, premium
->   SELECT u.first_name, u.last_name, l.Basic, l.Premium
	From users as u
	JOIN (
	   SELECT user_id,
	   SUM(CASE WHEN status = 2 THEN 1 ELSE 0 END) AS Basic, 
	   SUM(CASE WHEN status = 3 THEN 1 ELSE 0 END) AS Premium
	   FROM listings
	   GROUP BY user_id
	) AS l ON u.id = l.user_id
	WHERE u.status = 2

3. Show the same count as before but only if they have at least ONE premium listing
- Please return at least: first_name, last_name, basic, premium
->  SELECT u.first_name, u.last_name, l.Basic, l.Premium
	From users as u
	JOIN (
	   SELECT user_id,
	   SUM(CASE WHEN status = 2 THEN 1 ELSE 0 END) AS Basic, 
	   SUM(CASE WHEN status = 3 THEN 1 ELSE 0 END) AS Premium
	   FROM listings
	   GROUP BY user_id
	   HAVING Premium > 0
	) AS l ON u.id = l.user_id
	WHERE u.status = 2

4. How much revenue has each active vendor made in 2013
- Please return at least: first_name, last_name, currency, revenue
->  SELECT 
	u.first_name, 
	u.last_name, 
	(case when c.currencyType is null then 'USD' else c.currencyType end) as currency, 
	sum(case when c.revenue is null then 0 else c.revenue end) as revenue
	FROM users as u
	LEFT JOIN listings as l on u.id=l.user_id
	LEFT JOIN (
		SELECT listing_id, 
		currency as currencyType, 
		sum(price) as revenue
		FROM clicks
		WHERE (created BETWEEN '2013-01-01' AND '2014-01-01')
		GROUP BY listing_id, currencyType) as c on l.id=c.listing_id
	WHERE u.status = 2
	GROUP BY u.first_name, u.last_name, currency

5. Insert a new click for listing id 3, at $4.00
- Find out the id of this new click. Please return at least: id
->  INSERT INTO `clicks` VALUES (3,1,4.00,'USD',NOW())
	SELECT LAST_INSERT_ID();

6. Show listings that have not received a click in 2013
- Please return at least: listing_name
->  select name as listing_name
	from listings
	where id not in (
		select listing_id
		from clicks
		WHERE (created BETWEEN '2013-01-01' AND '2014-01-01') 
		group by listing_id
	)

7. For each year show number of listings clicked and number of vendors who owned these listings
- Please return at least: date, total_listings_clicked, total_vendors_affected
->  select lst.created as date, sum(lst.clickCount) as total_listings_clicked, Count(DISTINCT(lst.user_id)) as total_vendors_affected
	from (
		select l.id, l.user_id, l.name, c.created, c.listing_id, c.clickCount
		From listings as l
		join (
			select created, listing_id, count(*) as clickCount
			from clicks
			group by created, listing_id
		) as c on l.id=c.listing_id
	) as lst
	group by lst.created


8. Return a comma separated string of listing names for all active vendors
- Please return at least: first_name, last_name, listing_names
->  select u.first_name, u.last_name, l.listing_names
	from users as u
	left join (
		SELECT user_id, GROUP_CONCAT(name) as listing_names from listings group by user_id 
	) as l on u.id=l.user_id
	where u.status = 2