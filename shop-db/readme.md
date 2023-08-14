# shop-db

An EF Core project that provides access to an AWS managed postgres
instance to persist data for the long-term.

## Development

- The EFCore context gets its connection string from the environment
variable `SHOP_DB_CS`.
