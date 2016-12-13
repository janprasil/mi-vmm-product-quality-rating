import api from './api/reducer';
import app from './app/reducer';
import config from './config/reducer';
import device from './device/reducer';
import { combineReducers } from 'redux';
import { reducer as fileUpload } from 'redux-file-upload';

const configureReducer = () =>
  combineReducers({
    api,
    app,
    config,
    device,
    fileUpload
  });

export default configureReducer;
