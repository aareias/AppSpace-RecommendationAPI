# API Endpoint #1: `GET /api/recommendation/all-time`

### Query Parameters
- `media` (enum) – Media Type ('tvshow', 'movie' or 'documentary)
- `genres` (string, optional) – Preferred genres split by commas
- `keywords` (string, optional) - Keywords to search for if tv show or movie
- `topics` (string, options) - Topics to search for if documentary

### Response Example For

```
{
    "title": "string",
    "overview": "string",
    "genres": ["string"],
    "language": "string",
    "releaseDate": "2025-08-30",
    "website": "string",
    "keywords": ["string"],

    // if tv show
    "seasons": 1,
    "episodes": 10,
    "concluded": true
}
```

---

# API Endpoint #2: `GET /api/recommendation/upcoming`

### Query Parameters
- `until` (string, optional) – Latest date for release
- `keywords` (string, optional) - Keywords to search for
- `since` (date, optional) - Minimum date of release

### Response Example

```
{
    "title": "string",
    "overview": "string",
    "genres": ["string"],
    "language": "string",
    "releaseDate": "2025-08-30",
    "website": "string",
    "keywords": ["string"],
}
```

---

# API Endpoint #3: `GET /api/recommendation/theater/upcoming`

### Query Parameters
- `until` (string, optional) – Latest date for release
- `age-rate` (string, optional) - Age rate
- `genres` (string, optional) – Preferred genres split by commas

### Response Example

```
{
    "title": "string",
    "overview": "string",
    "genres": ["string"],
    "language": "string",
    "releaseDate": "2025-08-30",
    "website": "string",
    "keywords": ["string"],
}
```

---

# API Endpoint #4: `GET /api/billboard`

### Query Parameters
- `weeks` (int) – Number of weeks for billboard (minimum 1)
- `screens` (int) – Number of screens in theater

### Response Example

```
{
    "weekPlan": [
        {
            "startDate": "2025-09-01",
            "endDate": "2025-09-07",
            "screenMovies": [
                {
                    "movie": {
                        "id": 1,
                        "title": "Example Movie",
                        "releaseDate": "2025-08-30",
                        "originalLanguage": "en",
                        "adult": false,
                        "genres": [
                            { "id": 101, "name": "Action" },
                            { "id": 102, "name": "Adventure" }
                        ]
                    }
                }
            ]
        }
    ],
    "totalWeeks": 1,
    "totalMovies": 1
}
```

---

# API Endpoint #5: `GET /api/billboard/intelligent`

### Query Parameters
- `since` (date) – First date of billboard
- `until` (date) – Last date of billboard
- `BigRooms` (int) – Number of big screen rooms in theater
- `SmallRooms` (int) – Number of small screen rooms in theater
- `FilterByMostSuccessful` (bool) – Filter for most sold movies

### Response Example

```
{
    "weekPlan": [
        {
            "startDate": "2025-09-01",
            "endDate": "2025-09-07",
            "screenMovies": [
                {
                    "isBigRoom": true,
                    "movie": {
                        "id": 1,
                        "title": "Example Movie",
                        "releaseDate": "2025-08-30",
                        "originalLanguage": "en",
                        "adult": false,
                        "genres": [
                            { "id": 101, "name": "Action" },
                            { "id": 102, "name": "Adventure" }
                        ]
                    }
                }
            ]
        }
    ],
    "totalWeeks": 1,
    "totalMovies": 1
}
```

