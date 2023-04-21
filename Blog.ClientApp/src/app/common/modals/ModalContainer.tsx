import { observer } from "mobx-react-lite";
import { useStore } from "../../stores/store";
import { Modal } from "semantic-ui-react";

export default observer(function ModalContainer() {

    const { modalStore } = useStore()

    return (
        <Modal open={modalStore.modal.open} onClose={modalStore.closeModal} size='mini' dimmer='blurring'>
            <Modal.Content>
                {modalStore.modal.body}
            </Modal.Content>
        </Modal>
    )
})