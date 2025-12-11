<script lang="ts">
    import { clearCache } from "../../stores/cacheHandlerService";
    import { signOut } from "$lib/services/singOut";

    let { isOpen = $bindable(false) } = $props();

    async function confirmLogout() {
        localStorage.removeItem('user');
        clearCache();
        await signOut();
        isOpen = false;
        document.location.reload();
    }

    function cancelLogout() {
        isOpen = false;
    }

    function handleBackdropClick(e: MouseEvent) {
        if (e.target === e.currentTarget) {
            cancelLogout();
        }
    }
</script>

{#if isOpen}
    <div class="modal-backdrop" onclick={handleBackdropClick}>
        <div class="modal-content">
            <button class="close-btn" onclick={cancelLogout}>✕</button>
            
            <h2>Logga ut?</h2>
            <p class="confirmation-text">Är du säker på att du vill logga ut?</p>
            
            <div class="button-group">
                <button class="cancel-btn" onclick={cancelLogout}>
                    Nej, stanna kvar
                </button>
                <button class="confirm-btn" onclick={async () => await confirmLogout()}>
                    Ja, logga ut
                </button>
            </div>
        </div>
    </div>
{/if}

<style>
    .modal-backdrop {
        position: fixed;
        top: 0;
        left: 0;
        width: 100%;
        height: 100%;
        background: rgba(0, 0, 0, 0.5);
        display: flex;
        align-items: center;
        justify-content: center;
        z-index: 1000;
        backdrop-filter: blur(4px);
    }

    .modal-content {
        background: white;
        padding: 2rem;
        border-radius: 16px;
        box-shadow: 0 8px 32px rgba(0, 0, 0, 0.3);
        max-width: 400px;
        width: 90%;
        position: relative;
        animation: slideIn 0.3s ease-out;
    }

    @keyframes slideIn {
        from {
            transform: translateY(-20px);
            opacity: 0;
        }
        to {
            transform: translateY(0);
            opacity: 1;
        }
    }

    .close-btn {
        position: absolute;
        top: 1rem;
        right: 1rem;
        background: transparent;
        border: none;
        font-size: 1.5rem;
        cursor: pointer;
        color: #666;
        padding: 0;
        width: 2rem;
        height: 2rem;
        display: flex;
        align-items: center;
        justify-content: center;
        border-radius: 50%;
        transition: all 0.2s ease;
    }

    .close-btn:hover {
        background: #f0f0f0;
        color: #333;
    }

    h2 {
        margin-top: 0;
        margin-bottom: 1rem;
        color: #FF6B6B;
        text-align: center;
        font-size: 1.8rem;
    }

    .confirmation-text {
        text-align: center;
        color: #666;
        font-size: 1.1rem;
        margin-bottom: 2rem;
    }

    .button-group {
        display: flex;
        gap: 1rem;
        justify-content: center;
    }

    .cancel-btn,
    .confirm-btn {
        flex: 1;
        padding: 0.875rem;
        border-radius: 8px;
        font-size: 1rem;
        font-weight: bold;
        cursor: pointer;
        transition: all 0.2s ease;
        border: 2px solid;
    }

    .cancel-btn {
        background: white;
        color: #4ECDC4;
        border-color: #4ECDC4;
    }

    .cancel-btn:hover {
        background: linear-gradient(135deg, #a8ff78 0%, #00CED1 100%);
        color: white;
        transform: translateY(-2px);
        text-shadow: 1px 1px 2px rgba(0, 168, 170, 0.4),
        -1px -1px 2px rgba(0, 168, 170, 0.4),
        1px -1px 2px rgba(0, 168, 170, 0.4),
        -1px 1px 2px rgba(0, 168, 170, 0.4);
        box-shadow: 0 4px 12px rgba(78, 205, 196, 0.3);
    }

    .confirm-btn {
        background: white;
        color: #FF6B6B;
        border-color: #FF6B6B;
    }

    .confirm-btn:hover {
        background: linear-gradient(135deg, #FF6B6B 0%, #FFB347 100%);
        color: white;
        transform: translateY(-2px);
        text-shadow: 1px 1px 2px rgba(0, 0, 0, 0.2),
        -1px -1px 2px rgba(0, 0, 0, 0.2),
        1px -1px 2px rgba(0, 0, 0, 0.2),
        -1px 1px 2px rgba(0, 0, 0, 0.2);
        box-shadow: 0 4px 12px rgba(255, 107, 107, 0.3);
    }
</style>