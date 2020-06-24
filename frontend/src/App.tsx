import { Context, createElement, Element } from '@bikeshaving/crank';
import { injectGlobal } from 'emotion';
import { Layout } from './components/Layout';

export default function(this: Context): Element {
  return (
    <Layout>
      Hello world!
    </Layout>
  );
}

injectGlobal`
  body {
    --depth-0: #232931;
    --depth-1: #2E343C;
    --depth-2: #393E46;
    --accent: #ccbb4e;
    --accent-shadow: #a59743;
    --accent-text: #302d22;
    --text: #eeeeee;
  }
  body, html {
    color: var(--text);
    background-color: var(--depth-0);
    font-family: 'Lato';
    padding: 0;
    margin: 0;
  }

  @media screen and (prefers-color-scheme: light) {
    body {
      /* todo: light mode? */
    }
  }
`;