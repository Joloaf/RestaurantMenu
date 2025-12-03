import { ApiService } from "./apiService";

export interface Dish {
    id: number | null;
    name: string;
    foodPicture: string;
}



export class DishService {
    constructor(private apiService: ApiService) {}
    
    public async getDishById(dishId: number): Promise<Dish> {
        const response = await this.apiService.get<Dish>(`dishes/${dishId}`);
        return response as Dish;
    }

    public async createDish(dish: Dish, menuId: number): Promise<Dish> {
        const response = await this.apiService.post('dishes/', {dish, menuId });
        return response as Dish;
    }
}
