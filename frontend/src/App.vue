<template>
  <div id="app">
     <router-view></router-view>
  </div>
</template>

<script>
export default {
  name: 'App'
}
</script>

<style>
* {
  -webkit-tap-highlight-color: transparent;
  outline: none !important;
  caret-color: transparent;
}

*:focus {
  outline: none !important;
}

div:focus,
p:focus,
span:focus,
button:focus {
  caret-color: transparent;
}

input:focus,
textarea:focus {
  caret-color: auto;
}

.login-container,
.cadastro-container {
  user-select: none;
  -webkit-user-select: none;
  -moz-user-select: none;
}

input,
textarea {
  user-select: text;
  -webkit-user-select: text;
  -moz-user-select: text;
}
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  color: #2c3e50;
  margin: 0;
  padding: 0;
  min-height: 100vh;
  overflow-x: hidden;

}
body {
  margin: 0 !important;
  padding: 0 !important;
  overflow-x: hidden;
}
*, *::before, *::after {
  font-family: inherit;
}
</style>