export const APP_OFFLINE = 'APP_OFFLINE';
export const APP_ONLINE = 'APP_ONLINE';
export const APP_SET_LOCATION = 'APP_SET_LOCATION';
export const APP_SHOW_MENU = 'APP_SHOW_MENU';
export const APP_START = 'APP_START';
export const APP_STORAGE_LOAD = 'APP_STORAGE_LOAD';
export const DECREMENT_SEATS = 'DECREMENT_SEATS';
export const INCREMENT_SEATS = 'INCREMENT_SEATS';
export const LOGIN = 'LOGIN';
export const LOGOUT = 'LOGOUT';
export const TOGGLE_MENU = 'TOGGLE_MENU';
export const SELECT_REFERENCE = 'SELECT_REFERENCE';
export const CHANGE_SLIDER_VALUE = 'CHANGE_SLIDER_VALUE';

export const changeSliderValue = (slider, val) => ({
  type: CHANGE_SLIDER_VALUE,
  payload: { slider, val }
});

export const selectReference = (key) => ({
  type: SELECT_REFERENCE,
  payload: { key }
});

export const setLocation = (location) => ({
  type: APP_SET_LOCATION,
  payload: { location },
});

export const login = () => ({
  type: LOGIN
});

export const logout = () => ({
  type: LOGOUT
});

export const toggleMenu = () => ({
  type: TOGGLE_MENU
});

export const incrementSeats = () => ({
  type: INCREMENT_SEATS
});

export const decrementSeats = () => ({
  type: DECREMENT_SEATS
});

export const start = () =>
  () =>
    ({
      type: APP_START,
    });
