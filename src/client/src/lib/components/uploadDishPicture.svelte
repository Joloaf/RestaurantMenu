<script lang="ts">
import type { Dish } from '$lib/services/DishService';

    export let id: number;
    export let menuId: string | null;


</script>
<form method="post" enctype="multipart/form-data">
    <input type="file" name="picture" required />

    <input type="hidden" name="type" value="dish" />
    <input type="hidden" name="menuId" value={menuId} />
    <input type="hidden" name="id" value={id} />

    <button>Upload</button>
</form>