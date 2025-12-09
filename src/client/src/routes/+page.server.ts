    import { dev } from "$app/environment";
    import type { Actions } from "@sveltejs/kit";
	import { writeFileSync } from "fs";

export const actions: Actions = {
    default: async ({ request }) => {
        const data = await request.formData();

        const files = data.getAll("picture") as File[];

        let path = "uploads/";
        if (dev) {
            path = "static/uploads/";
        }

        for (const file of files) {
            const fileName = crypto.randomUUID();
            const bytes = Buffer.from(await file.arrayBuffer());
            await writeFileSync(path + fileName, bytes);
        }

        return { success: true };
    }
};