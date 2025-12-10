import { ApiService } from "./apiService";
import { apiUrlResolver } from "./apiService"
import { clearCache } from "../../stores/cacheHandlerService";
const apiService = new ApiService()


export async function signOut(){
    await apiService.post(`logout`, null)
    clearCache()
}