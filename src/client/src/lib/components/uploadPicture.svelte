<script lang="ts">
    // Returns the base64 data URL via callback
    
    export let onPictureSelected: (pictureDataUrl: string) => void;
    export let buttonText: string = "Upload Picture";
    
    let fileinput: HTMLInputElement;

    const onFileSelected = (e: Event) => {
        const target = e.target as HTMLInputElement;
        const image = target.files?.[0];
        if (!image) return;
        
        let reader = new FileReader();
        reader.readAsDataURL(image);
        reader.onload = (e) => {
            if (e?.target?.result) {
                const pictureDataUrl = e.target.result as string;
                onPictureSelected(pictureDataUrl);
            }
        };
    };

</script>

<div class="upload-container">
    <!-- svelte-ignore a11y_no_noninteractive_element_interactions -->
    <!-- svelte-ignore a11y_click_events_have_key_events -->
    <img 
        class="upload-icon" 
        src="../pictures/625182-200.png" 
        alt="Upload" 
        onclick={() => fileinput.click()}
    />
    <!-- svelte-ignore a11y_no_static_element_interactions -->
    <!-- svelte-ignore a11y_click_events_have_key_events -->
    <div class="upload-text" onclick={() => fileinput.click()}>
        {buttonText}
    </div>
    <input 
        type="file" 
        accept=".jpg, .png, .jpeg, .webp"  
        style="display:none" 
        onchange={onFileSelected} 
        bind:this={fileinput}
    />
</div>

<style>
    .upload-container {
        display: flex;
        align-items: center;
        gap: 0.5rem;
        cursor: pointer;
    }
    
    .upload-icon {
        width: 24px;
        height: 24px;
        cursor: pointer;
    }
    
    .upload-text {
        cursor: pointer;
        user-select: none;
    }
    
    .upload-text:hover {
        text-decoration: underline;
    }
</style>
