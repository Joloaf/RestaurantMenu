
import { goto } from '$app/navigation';
import { PUBLIC_API_URL } from '$env/static/public'
// import {isAuthenticated} from "$lib/stores/authStore";


//const apiDevUrl = import.meta.en // this one might fail
const apiDevUrl = PUBLIC_API_URL;

interface AppError {
    code: string;
    message: string;
    details?: unknown;
}

function handleAuthError(resopnse: Response) {
    if(resopnse.status === 401 || resopnse.status === 403) {
        // isAuthenticated.set(false);
        goto('/login');
        return true;
    }
    return false;
}

export class ApiService {
    public async post<T,D>(endpoint: string, data: D): Promise<T | AppError> {
        try{
            const response = await fetch (`${apiDevUrl}/${endpoint}`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(data)
            });
            if(!response.ok) {
				if (handleAuthError(response)) {
					return {
						code: 'AUTH_ERROR',
						message: 'Authentication required. Redirecting to login.',
						details: response.status
					};
				}
                return {
                    code: 'API_ERROR',
                    message: `Failed to post to ${apiDevUrl}}/${endpoint}`,
                    details: await response.text()
                };
            }
            const responseData = await response.json();
            return responseData;
        }
        catch(error) {
            return {
                code: 'NETWORK_ERROR',
                message: `Network error while posting to ${apiDevUrl}}/${endpoint}`,
                details: error
            };
        }
    }
    public async get<T>(endpoint: string,
        params?: Record<string, string | number | undefined>
    ): Promise<T | AppError> {
        try{
            let url = `${apiDevUrl}/${endpoint}`;
            if(params) {
                const quesry= Object.entries(params)
                .filter(([_, value]) => value !== undefined)
                .map(([key, value]) => `${encodeURIComponent(key)}=${encodeURIComponent(String(value))}`)
                .join('&');
                url += `?${quesry}`;
            }
			const response = await fetch(url);
            if(!response.ok){
                if(handleAuthError(response)) {
                    return {
                        code: 'AUTH_ERROR',
                        message: 'Authentication required. Redirecting to login.',
                        details: response.status
                    };
                }
                if(response.status === 404) {
                    return {
                        code: 'NOT_FOUND',
                        message: `Resource not found at ${apiDevUrl}/${endpoint}`,
                        details: response.status
                    };
                }
                return {
                    code: 'API_ERROR',
                    message: `Failed to get from ${apiDevUrl}/${endpoint}`,
                    details: await response.text()
                };
            }
            return await response.json();
        }
        catch(error) {
            return {
                code: 'NETWORK_ERROR',
                message: `Network error while getting from ${apiDevUrl}/${endpoint}`,
                details: error
            };
        }
    }
    public async patch<T,D>(endpoint: string, data: D): Promise<T | AppError> {
    try{
        const response = await fetch (`${apiDevUrl}/${endpoint}`, {
            method: 'PATCH',
            headers: {
                'Content-Type': 'application/json'
            },
            credentials: 'include',
		body: JSON.stringify(data)
        });
        if(!response.ok) {
            if(handleAuthError(response)){
                return {
                    code: 'AUTH_ERROR',
                    message: 'Authentication required. Redirecting to Login.',
                    details: response.status
                };
            }
            return {
                code: 'API_ERROR',
                message: `Failed to patch to ${apiDevUrl}/${endpoint}`,
                details: await response.text()
            };
        }
        const responseData = await response.json();
        return responseData;

    } catch(error) {
        return {
            code: 'NETWORK_ERROR',
            message: `Network error while patching to ${apiDevUrl}/${endpoint}`,
            details: error
        }
    }
    };
    public async delete<T>(endpoint: string): Promise<T | AppError> {
        try{
            const response = await fetch (`${apiDevUrl}/${endpoint}`, {
                method: 'DELETE',
                credentials: 'include',
            });
            if(!response.ok) {
                if(handleAuthError(response)){
                    return {
                        code: 'AUTH_ERROR',
                        message: 'Authentication required. Redirecting to Login.',
                        details: response.status
                    };
                }
                return {
                    code: 'API_ERROR',
                    message: `Network error while patching to ${apiDevUrl}/${endpoint}`,
                    details: await response.text()
                };
            }
            const responseData = await response.json();
            return responseData;
        }
        catch(error) {
            return {
                code: 'NETWORK_ERROR',
                message: `Network error while deleting to ${apiDevUrl}/${endpoint}`,
                details: error
            }
        }
    }
}
            

