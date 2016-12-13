import express from 'express';
import modRewrite from 'connect-modrewrite';

const app = express();

const appUrl = process.env.APP_URL;

app.use(modRewrite([
  `^/webapi/(.*)$ ${appUrl}/api/$1 [P]`,
  `^/assets/(.*)$ ${appUrl}/$1 [P]`
]));

app.on('mount', () => {
  console.log('WebAPI proxy is available at /webapi');
});

export default app;
