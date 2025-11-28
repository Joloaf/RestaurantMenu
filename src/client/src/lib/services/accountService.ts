import { ApiService } from "./apiService";




export interface RegisterRequest {
    Username: string;
    Password: string;
    Email: string;
}
export interface LoginRequest {
    Password: string;
    Username: string;
}

export interface UserResponse {
    Id: string;
    Username: string;
    Email: string;
}
export interface ErrorResponse {
    Message:string
}


export class AccountService {
    constructor(private apiService: ApiService){}

    public async registerAccount(account: RegisterRequest): Promise<UserResponse | ErrorResponse> {
        const response = await this.apiService.post('Account/register', account);
        return response as UserResponse | ErrorResponse;
    }

    public async loginAccount(request: LoginRequest): Promise<UserResponse | ErrorResponse> {
        const response = await this.apiService.post('Account/login', request);
        return response as UserResponse | ErrorResponse;
    }
}
