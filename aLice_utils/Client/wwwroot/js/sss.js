export class SSS {
    async address() {
        try {
            await navigator.clipboard.writeText(window.SSS.activeAddress);
        } catch (error) {
            console.error('Failed to write text to clipboard:', error);
        }
    }
    
    async getPublicKey(){
        try {
            await navigator.clipboard.writeText(window.SSS.activePublicKey);
        } catch (error) {
            console.error('Failed to write text to clipboard:', error);
        }
    }
    
    async signTransaction(serializedTx){
        window.SSS.setTransactionByPayload(serializedTx);
        return await window.SSS.requestSign();
    }

    static create() {
        return new SSS();
    }
    
    async openLink(link){
        window.open(link, '_blank');
    }
}