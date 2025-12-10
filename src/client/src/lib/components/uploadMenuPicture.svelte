<script lang="ts">
export let id: string | null;


</script>
<form method="post" enctype="multipart/form-data">
    <input class="upload" type="file" name="picture" />

    <input type="hidden" name="type" value="menu" />
    <input type="hidden" name="id" value={id} />

    <button>Upload</button>
</form>
<style>
    .upload{
        border-style: solid;
        border-width: 1px;
        border-color: gray;
        padding: 4px;

    }
</style>
