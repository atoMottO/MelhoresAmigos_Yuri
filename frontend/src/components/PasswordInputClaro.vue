<template>
  <div class="form-group"> 
    
    <label v-if="label" :for="id">{{ label }}</label> 

    <div class="input-container">
      <input 
        :id="id"
        :type="input_type" 
        class="password-input-field"
        :placeholder="placeholder"
        :value="modelValue"
        @input="$emit('update:modelValue', $event.target.value)"
        @keyup.enter="$emit('enter')"
      />
      
      <button @click="obfuscateToggle" class="toggle-button" type="button" tabindex="-1">
        <i :class="eyeIconClasses"/> 
      </button>
    </div>

    <small v-if="hint">{{ hint }}</small> 
  </div>
</template>

<script>
export default {
  name: "PasswordToggleInput",
  emits: ['update:modelValue', 'enter'],
  data() {
    return {
      input_type: this.type,
      passwordVisible: false,
      id: `password-input-${Math.random().toString(36).substring(2, 9)}`, 
    };
  },
  props: {
    modelValue: { type: String, default: '' }, 
    type: { type: String, default: "password" },
    label: { type: String, default: 'Senha' },
    placeholder: { type: String, default: '' },
    hint: { type: String, default: '' },
  },
  computed: {
    eyeIconClasses() {
      return {
        'fas': true,
        'fa-eye': this.passwordVisible, 
        'fa-eye-slash': !this.passwordVisible,
      };
    },
  },
  methods: {
    obfuscateToggle() {
      this.passwordVisible = !this.passwordVisible;
      this.input_type = this.passwordVisible ? "text" : "password";
    },
  },
};
</script>

<style scoped>
.form-group {
  margin-bottom: 20px;
  text-align: left;
}

.form-group label {
  display: block;
  font-size: 14px;
  font-weight: 500;
  margin-bottom: 8px;
  color: #333333;
}

.form-group small {
  display: block;
  font-size: 12px;
  margin-top: 6px;
  color: #666666;
}

.input-container {
  position: relative;
  width: 82%;
}

.input-container .password-input-field {
  width: 100%;
  padding: 14px 45px 14px 16px; 
  
  background-color: #ffffff;
  color: #333333;
  
  border: 1.5px solid #e7e7e7;
  
  border-radius: 10px; 
  font-size: 15px;
  font-family: inherit;
  transition: all 0.2s;
  
  &::placeholder { 
    color: #999999;
  }
}

.input-container .password-input-field:focus {
  outline: none;
  border-color: #999999;
}

.toggle-button {
  position: absolute;
  top: 50%;
  right: 1px;
  left: 103%;
  transform: translateY(-50%);
  
  background: none;
  border: none;
  cursor: pointer;
  padding: 8px;
  color: #666666; 
  font-size: 16px;
  transition: color 0.2s;
}

.toggle-button:hover {
  color: #333333;
}
</style>