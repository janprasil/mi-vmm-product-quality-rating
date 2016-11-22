import configureReducer from './configureReducer';
import configureMiddleware from './configureMiddleware';
import createSagaMiddleware from 'redux-saga';
import { applyMiddleware, createStore } from 'redux';
import saga from './sagas';

type Options = {
  initialState: Object,
  platformDeps?: Object,
  platformMiddleware?: Array<Function>,
};

const configureStore = (options: Options) => {
  const {
    initialState,
    platformDeps = {},
    platformMiddleware = [],
  } = options;

  const reducer = configureReducer(initialState);


  const sagaMiddleware = createSagaMiddleware();
  const middleware = configureMiddleware(
    initialState,
    platformDeps,
    platformMiddleware,
    sagaMiddleware
  );

  const store = createStore(
    reducer,
    initialState,
    applyMiddleware(...middleware)
  );

  // Enable hot reloading for reducers.
  if (module.hot) {
    if (initialState.device.isReactNative) {
      // React Native for some reason needs accept without the explicit path.
      // facebook.github.io/react-native/blog/2016/03/24/introducing-hot-reloading.html
      module.hot.accept(() => {
        const configureReducer = require('./configureReducer').default;

        store.replaceReducer(configureReducer(initialState));
      });
    } else {
      // Webpack for some reason needs accept with the explicit path.
      module.hot.accept('./configureReducer', () => {
        const configureReducer = require('./configureReducer').default;

        store.replaceReducer(configureReducer(initialState));
      });
    }
  }

  sagaMiddleware.run(saga);

  return store;
};

export default configureStore;
