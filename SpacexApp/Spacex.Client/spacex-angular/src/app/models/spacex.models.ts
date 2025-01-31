export interface Result<T> {
    value: T;
    isSuccess: boolean;
    isFailure: boolean;
    isEmpty: boolean;
    message: string;
    httpStatusCode: number;
    resultType: ResultType;
  }

  export enum ResultType {
    InternalError = 0,
    Ok = 1,
    NotFound = 2,
    Forbidden = 3,
    Conflicted = 4,
    Invalid = 5,
    Unauthorized = 6
  }

  export interface SignUpRequest{
    firstName: string;
    lastName: string;
    email: string;
    password: string;
  }