<template>
  <div v-if="isOpen" class="modal-overlay" @click="fechar">
    <div class="modal-container" @click.stop>
      <div class="modal-header">
        <h3>Confirmar Saída</h3>
      </div>
      
      <div class="modal-body">
        <p>Tem certeza que deseja sair e encerrar sua sessão?</p>
      </div>
      
      <div class="modal-footer">
        <button class="btn-cancelar" @click="fechar">
          Cancelar
        </button>
        <button class="btn-confirmar" @click="confirmar">
          Sair
        </button>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
export default {
  props: {
    isOpen: {
      type: Boolean,
      required: true
    }
  },
  emits: ['confirmar', 'fechar'],
  methods: {
    confirmar() {
      this.$emit('confirmar');
    },
    fechar() {
      this.$emit('fechar');
    }
  }
}
</script>

<style scoped>
.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background-color: rgba(0, 0, 0, 0.5);
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 9999;
}

.modal-container {
  background: white;
  border-radius: 8px;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
  max-width:500px;
  width: 90%;
  animation: slideIn 0.3s ease-out;
}

@keyframes slideIn {
  from {
    transform: translateY(-50px);
    opacity: 0;
  }
  to {
    transform: translateY(0);
    opacity: 1;
  }
}

.modal-header {
  padding: 1.5rem;
  border-bottom: 1px solid #e5e7eb;
}

.modal-header h3 {
  margin: 0;
  font-size: 1.25rem;
  font-weight: 600;
  color: #111827;
}

.modal-body {
  padding: 1.5rem;
}

.modal-body p {
  margin: 0;
  color: #6b7280;
  font-size: 1rem;
  line-height: 2.5;
}

.modal-footer {
  padding: 1rem 1.5rem;
  border-top: 1px solid #e5e7eb;
  display: flex;
  justify-content: flex-end;
  gap: 0.75rem;
}

.btn-cancelar,
.btn-confirmar {
  padding: 0.5rem 1.25rem;
  border-radius: 6px;
  font-size: 0.875rem;
  font-weight: 500;
  cursor: pointer;
  transition: all 0.2s;
  border: none;
}

.btn-cancelar {
  background-color: #f3f4f6;
  color: #374151;
}

.btn-cancelar:hover {
  background-color: #e5e7eb;
}

.btn-confirmar {
  background-color: #ef4444;
  color: white;
}

.btn-confirmar:hover {
  background-color: #dc2626;
}

.btn-cancelar:focus,
.btn-confirmar:focus {
  outline: 2px solid #3b82f6;
  outline-offset: 2px;
}
</style>