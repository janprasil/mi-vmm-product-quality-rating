import express from 'express';
import modRewrite from 'connect-modrewrite';

const app = express();

const appUrl = process.env.APP_URL;

if (!appUrl)
  console.log('APP_URL is not set!');

app.use(modRewrite([
  `^/webapi/(.*)$ ${appUrl}/api/$1 [P]`,
  `^/uploads/(.*)$ ${appUrl}/uploads/$1 [P]`
]));

app.on('mount', () => {
  console.log('WebAPI proxy is available at /webapi');
});

export default app;
