// Styles
import "@mdi/font/css/materialdesignicons.css";
import "vuetify/styles";

// Vuetify
import { createVuetify } from "vuetify";
import * as components from "vuetify/components";
import * as directives from "vuetify/directives";
import { aliases, mdi } from "vuetify/iconsets/mdi";

export default createVuetify({
  components,
  directives,
  icons: {
    defaultSet: "mdi",
    aliases,
    sets: {
      mdi,
    },
  },
  theme: {
    defaultTheme: "light",
    themes: {
      light: {
        dark: false,
        colors: {
          primary: "#428ecc",
          secondary: "#c1dff7",
          error: "#f84949",
          info: "#2196F3",
          success: "#4CAF50",
          warning: "#eb7628",
          surface: "#FFFFFF",
          background: "#eeeeee",
          white: "#FFFFFF",
        },
      },
      dark: {
        dark: true,
        colors: {
          primary: "#428ecc",
          secondary: "#c1dff7",
          error: "#f84949",
          info: "#2196F3",
          success: "#4CAF50",
          warning: "#eb7628",
          surface: "#212121",
          background: "#121212",
          white: "#FFFFFF",
        },
      },
    },
  },
  defaults: {
    VTextField: {
      variant: "outlined",
      density: "compact",
      validateOn: "blur",
      rounded: true,
    },
    VSelect: {
      variant: "outlined",
      density: "compact",
      validateOn: "blur",
      rounded: true,
    },
    VAutocomplete: {
      variant: "outlined",
      density: "compact",
      validateOn: "blur",
      rounded: true,
    },
    VTextarea: {
      variant: "outlined",
      density: "compact",
      validateOn: "blur",
      rounded: "lg",
    },
    VCheckbox: {
      density: "compact",
      validateOn: "blur",
      rounded: true,
    },
    VRadio: {
      density: "compact",
      validateOn: "blur",
    },
    VRadioGroup: {
      density: "compact",
      validateOn: "blur",
    },
    VBtn: {
      variant: "flat",
      rounded: "pill",
      density: "default",
      elevation: 0,
      // style: 'height: 35px; font-size:14px;',
    },
    VCard: {
      elevation: 0,
    },
    VCardActions: {
      VBtn: {
        variant: "flat",
      },
    },
    VSwitch: {
      color: "primary",
      density: "compact",
    },
    VSlider: {
      color: "primary",
    },
    VAlert: {
      variant: "tonal",
      density: "compact",
      rounded: "lg",
      border: "start",
    },
  },
});
