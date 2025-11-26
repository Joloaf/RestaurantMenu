import { ApiService } from "./apiService";

export interface Dish {
    id: number;
    name: string;
    foodPicture: string;
}



export class DishService {
    constructor(private apiService: ApiService) {}
    
    public async getDishById(dishId: number): Promise<Dish> {
        const response = await this.apiService.get<Dish>(`dishes/${dishId}`);
        return response as Dish;
    }
}
