>[!IMPORTANT]
>Project by Joel Hellberg 2024

# FilmRecensioner API Documentation
>[!NOTE]
>Below you will find the documentation for this project.

### API Structure

The API is divided into two main controllers: ReviewsController and VideosController.

Each controller handles its respective data objects (Review and Video). 

Both controller classes are decorated with [ApiController] and use [Authorize] to require authentication for certain actions.
<br>

## Test Checklist

>[!NOTE]
>I've listed the CRUD and Authentication tests below
- [x] Registration
- [x] Logging in
- [x] GET All Videos (Both unathorized and authorized users)
- [x] GET Video by Id (Both unathorized and authorized users)
- [x] POST Video (Authorized users only)
- [x] PUT Video (Authorized users only)
- [x] DELETE Video (Authorized users only) 
- [x] GET All Reviews (Both unathorized and authorized users)
- [x] GET Review by Id (Both unathorized and authorized users)
- [x] POST Review (Authrized users only)
- [x] PUT Review (Authorized users only)
- [x] DELETE Review (Authorized users only)
- [x] User cannot edit/delete another users Reviews
<br>

## Data Models and Database

### Review DTO

Used to transfer review data between the client and server.
<br>

### Video DTO

Used to transfer video data between the client and server.
<br>

### ApplicationDbContext

Database context inheriting from IdentityDbContext, containing DbSet for Videos and Reviews.
<br>
<br>
# API Endpoints
>[!NOTE]
>Below is a full documentation of each endpoint and what it does.
<br>

# Video Endpoints
>[!NOTE]
> All API Endpoints for Video
<br>

## 1. Get all Videos

>[!NOTE]
>Description: Retrieves all videos with associated reviews.

>[!TIP]
>Method: GET /api/Videos

Response:

Success:

    "Id": 456,
    "Title": "The Dark Knight",
    "Date": "2023-02-01T00:00:00",
    "Reviews": [
      {
        "Id": 2,
        "Rating": 5,
        "ReviewText": "Amazing storyline!",
        "UserId": "user456"
      }

No Videos Found:
        
    "No videos were found, try adding one."
<br>

## 2. Get Video by ID

>[!NOTE]
>Description: Retrieves a specific video with associated reviews based on ID.

>[!TIP]
>Method: GET
>/api/Videos/{id}

Response: 

Success

    "Id": 123,
    "Title": "Inception",
    "Date": "2023-01-01T00:00:00",
    "Reviews": [
      {
        "Id": 1,
        "Rating": 4,
        "ReviewText": "Great movie!",
        "UserId": "user123"
      }

Invalid ID

    "Id must be higher than 0"

Video Not Found

    "No video was found with that id"
<br>

## 3. Update Video

>[!NOTE]
>Description: Updates a video, requires authentication.

>[!WARNING]
>Method: PUT
>/api/Videos/{id}

Response: 

Success

    204 No Content (The content is updated)

Invalid ID

    "Id must be higher than 0"

Video not found

    "No video was found with that id"

Unauthorized

    "You are not allowed to change another user´s video"

<br>

## 4. Create Video

>[!NOTE]
>Description: Creates a new video, requires authentication.

>[!WARNING]
>Method: POST /api/Videos

Response:

Success

    "Id": 789,
    "Title": "Interstellar",
    "Date": "2023-03-01T00:00:00" / Date as of creation

Invalid Date

    "Date is required and can't be the default value."
<br>

## 5. Delete Video

>[!NOTE]
>Description: Deletes a video, requires authentication.

>[!CAUTION]
>Method: DELETE /api/Videos/{id}

Response: 

Success

    204 No Content

Invalid ID

    "Id must be higher than 0"

Video not found

    "No video was found with that id"
<br>

# Review Endpoints
>[!NOTE]
> All API Endpoints for Review
<br>

## 1. Get Reviews

>[!NOTE]
>Description: Retrieve a list of reviews with associated video information.

>[!TIP]
>Method: GET /api/Reviews

Response:

Success

    {
    "Id": 1,
    "Rating": 4,
    "ReviewText": "Great movie!",
    "VideoTitle": "Inception",
    "VideoId": 123,
    "UserId": "user123"
    },
    {
    "Id": 2,
    "Rating": 5,
    "ReviewText": "Amazing storyline!",
    "VideoTitle": "The Dark Knight",
    "VideoId": 456,
    "UserId": "user456"
    }

No Reviews found

    "No reviews were found, try adding one."
<br>

## 2. Get Review by ID

>[!NOTE]
>Description: Retrieves a specific review with associated video information based on ID.

>[!TIP]
>Method: GET /api/Reviews/{id}

Response:

Success

    {
    "Id": 1,
    "Rating": 4,
    "ReviewText": "Great movie!",
    "Videoname": "Inception",
    "VideoId": 123,
    "UserId": "user123"
    }

Invalid ID 

    "Id must be higher than 0"

Review not found

    "There is no review with that ID"
<br>

## 3. Update Review

>[!NOTE]
>Description: Updates a review, requires authentication.

>[!WARNING]
>Method: PUT /api/Reviews/{id}

Response:

Success

    204 No Content

Invalid ID 

    "Id must be higher than 0"

Review Not Found

    "There is no review with that ID"

Unauthorized

    "You are not allowed to change another user's review"

Video Not Found

    "No video was found with that id"   
<br>

## 4. Create Review

>[!NOTE]
>Description: Creates a new review, requires authentication.

>[!WARNING]
>Method: POST /api/Reviews

Response:

Success

    "Id": 3,
    "Rating": 4,
    "ReviewText": "Excellent movie!",
    "Videoname": "Interstellar",
    "VideoId": 789,
    "UserId": "user789"

Invalid Video ID

    "VideoId must be higher than 0"

Video Not Found

    "No video was found with that id"
<br>

## Delete Review

>[!NOTE]
>Description: Deletes a review, requires authentication.

>[!CAUTION]
>Method: DELETE /api/Reviews/{id}

Response:

Success

    204 No Content

Invalid ID

    "Id must be higher than 0"

Review Not Found

    "Review not found. Try searching for another one."   
<br>

## Get Reviews for a Video

>[!NOTE]
>Description: Retrieves reviews for a specific video.

>[!TIP]
>Method: GET /api/Reviews/GetReviewsForVideo/{videoId}

Response:

Success

    "Id": 1,
    "Rating": 4,
    "ReviewText": "Great movie!",
    "Videoname": "Inception",
    "VideoId": 123,
    "UserId": "user123"

Invalid Video ID

    "Id must be higher than 0"

No Reviews Found for Video

    "No reviews were found for that video"

Video Not Found

    "No video was found with that id"
<br>

## Get Reviews for Current User

>[!NOTE]
>Description: Retrieves reviews for the authenticated user.

>[!TIP]
>Method: GET /api/Reviews/GetReviewsForUser

Response:

Success

    "Id": 1,
    "Rating": 4,
    "ReviewText": "Great movie!",
    "Videoname": "Inception",
    "VideoId": 123,
    "UserId": "user123"

No Reviews Found for User

    "No reviews were found from that user"
<br>

# Analysis and Reflection

## Performance
When it comes to how fast the API works, using techniques like asynchronous methods and Entity Framework helps it perform well. To make it even faster, we could add a system called caching. This helps by saving some information, so the API doesn't have to check the database every time, making things quicker.

## Scalability
If we want the API to handle more users or data in the future, we can make it more scalable. This involves using some tricks like caching, choosing databases that can handle a lot of information, and using a CDN (content delivery network) to share static content faster. We might also think about balancing the workload and using smaller, specialized parts of the API to manage increased demand.

## Security
To keep things secure, the API already checks if you're allowed to do certain things by using your username and password. But we could do more! Adding things like a secure connection (HTTPS), and protection against sneaky attacks (like SQL injection) would make it even more secure.

## Maintainability
The way the code is organized makes it easy to understand and work with. We use some modern tools to manage the database, and we have clear ways to send data back and forth between the website and the server. The documentation, which is like a guide for using the API, helps us keep everything in good shape and make changes without causing problems.

# After thoughts
In general i thought this was a good challenge, using the knowledge from the last API project and the video material from both Udemy and Linus on canvas made this project easier to complete.
But in honesty it was a headache at sometimes. I am now looking forward to the next part where we will add a UI.
