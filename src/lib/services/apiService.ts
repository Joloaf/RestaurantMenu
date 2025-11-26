export interface Menu {
    menuId: number;
    menuName: string;
    userName: string;
    theme: string;
    userId: string;
    dishes: Dish[];
}

export interface Dish {
    id: number;
    name: string;
    foodPicture: string;
}
