/* eslint-disable quotes */
import express from 'express';

const app = express();

app.on('mount', () => {
  console.log('API is at %s', app.mountpath);
});

export default app;
