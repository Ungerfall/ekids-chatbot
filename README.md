# ekids-chatbot: 

### Homework Checker Module

The _homework checker module_ is designed to automate the evaluation of SCRATCH projects using the OpenAI Chat Completion API (https://platform.openai.com/docs/api-reference/chat/object).

#### Application Flow:

1. **Monitor Directory**: This application monitors a specific directory for new SCRATCH project files (sb3 files) with a filename pattern of `[HomeworkCode]_[StudentName].sb3`.
2. **Retrieve Metadata**: It extracts the homework code from the filename and queries a database to retrieve associated metadata, including project description and evaluation criteria.
3. **Construct a Prompt**: The system constructs a clear and concise prompt that includes the criteria obtained from the database. This prompt is then sent to the Chat Completion API.
4. **API Request**: The constructed prompt is sent as a request to the OpenAI Chat Completion API, which returns a response with insights about the SCRATCH project's compliance with the specified criteria.
5. **Parse API Response**: The application parses the response from the API to extract relevant information and insights about the SCRATCH project's compliance. It also saves the token usage to the database for future audit purposes
6. **Decision-Making Logic**: Based on the API response, the system implements decision-making logic to determine whether the SCRATCH project meets the specified criteria. This helps in automatically grading and evaluating the projects. The application also saves the evaluation results to the database
7. **Move Files**: After evaluation, all homework files, whether they pass the criteria or not, are moved to a separate directory. This prevents re-processing of the same files, ensuring that each file is evaluated only once.

#### Function Call
To enable the Chat Completion API to intelligently generate a JSON response, a function call has been introduced. The following is a sample function call pattern:

```
get_evaluation = [
    {
        'name': 'get_evaluation',
        'description': 'Get the homework evaluation based on the given criteria',
        'parameters': {
            'type': 'object',
            'properties': {
                'success': {
                    'type': 'boolean',
                    'description': 'A flag to determine if the criteria is met.'
                },
                'criteria': {
                    'type': 'string',
                    'description': 'The criteria currently being evaluated.'
                },
                'sprite': {
                    'type': 'string[]',
                    'description': 'A list of sprites that meet the criteria.'
                }
            }
        }
    }
]
```

### Homework Management Module
The _homework management module_ provides the following REST APIs to manage homework metadata.

| Method |                             End Point                              |              Description               |
|--------|:------------------------------------------------------------------:|:--------------------------------------:|
| POST   |             /homework-management/authentication/users              |             Register User              |
| POST   |             /homework-management/authentication/login              |               User Login               | 
| POST   |         /homework-management/homework-management/homework          |            Create Homework             | 
| GET    |            /homework-management/homework/[homework_id]             |           Get Homework By Id           | 
| PUT    |        /homework-management/homework/[homework_id]/criteria        | Add or Update a Criteria to a homework | 
| DELETE | /homework-management/homework/[homework_id]/criteria/[criteria_id] |            Delete Criteria             | 
| DELETE |            /homework-management/homework/[homework_id]             |            Delete Homework             |

#### Database Design

![ekids Homework Checking DB.png](src%2Fhomework-management-backend%2Fekids%20Homework%20Checking%20DB.png)

