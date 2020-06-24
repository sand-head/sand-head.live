import App from './App';
import express from 'express';
import { createElement } from '@bikeshaving/crank';
import { renderer } from '@bikeshaving/crank/cjs/html';
import { extractCritical } from 'emotion-server';
import { ServerSignalRProvider } from './components/SignalR';

const assets = require(process.env.RAZZLE_ASSETS_MANIFEST!);

const server = express();
server
  .disable('x-powered-by')
  .use(express.static(process.env.RAZZLE_PUBLIC_DIR!))
  .get('/*', async (req, res) => {
    const html = await renderer.render(
      <ServerSignalRProvider>
        <App />
      </ServerSignalRProvider>);
    const styles = extractCritical(html);

    res.status(200).send(
      `<!doctype html>
    <html lang="">
    <head>
        <meta http-equiv="X-UA-Compatible" content="IE=edge" />
        <meta charSet='utf-8' />
        <title>sand_head</title>
        <meta name="viewport" content="width=device-width, initial-scale=1">
        <link href="https://fonts.googleapis.com/css2?family=Lato:ital,wght@0,400;0,700;1,400&family=Solway:wght@700&display=swap" rel="stylesheet">
        ${assets.client.css ?
          `<link rel="stylesheet" href="${assets.client.css}">` : ''}
        <style data-emotion-css="${styles.ids.join(' ')}">
          ${styles.css}
        </style>
        ${process.env.NODE_ENV === 'production' ?
          `<script src="${assets.client.js}" defer></script>` :
          `<script src="${assets.client.js}" defer crossorigin></script>`}
    </head>
    <body>
        ${html}
    </body>
</html>`
    );
  });

export default server;