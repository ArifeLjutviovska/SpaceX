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

  export interface UpdatePasswordRequest{
    currentPassword: string;
    newPassword: string;
  }

  export interface CurrentUserResponse {
    firstName: string;
    lastName: string;
    email: string;
  }
  export interface ResetPasswordRequest{
    email: string;
    newPassword: string;
  }

  export interface LoginRequest {
    email: string;
    password: string;
  }

  export interface LoginResponse {
    accessToken: string;
    refreshToken: string;
  }

  export interface SpaceXLaunchDto {
    id: string;
    name: string;
    dateUtc: string;
    rocketId?: string;
    success?: boolean;
    details?: string;
    patchImage?: string;
    webcastUrl?: string;
    wikipediaUrl?: string;
    articleUrl?: string;
    launchpad?: string;
  }
  
  
  export interface PagedResult<T> {
    items: T[];
    totalItems: number;
    totalPages: number;
    currentPage: number;
  }