import { HubConnectionBuilder } from '@microsoft/signalr';
import { Context, Children } from '@bikeshaving/crank';

export const SIGNALR_KEY = 'SignalRClient';

interface ServerSignalRProviderProps {
  children: Children;
}
type SignalRProviderProps = ServerSignalRProviderProps & {
  url: string;
};

export interface SignalRClient {
  on: (key: string, callback: (...args: any[]) => void) => void;
  off: (key: string, callback: (...args: any[]) => void) => void;
  send: (key: string, ...args: any[]) => Promise<void>;
}

export function BrowserSignalRProvider(this: Context, { children, url }: SignalRProviderProps) {
  console.log('connecting to hub', url);
  const connection = new HubConnectionBuilder()
    .withUrl(url)
    .build();
  connection.start().catch(err => {
    console.error('signal r error occurred', err);
  });
  this.set(SIGNALR_KEY, connection);
  return children;
}

export function ServerSignalRProvider(this: Context, { children }: ServerSignalRProviderProps) {
  // we don't actually want to connect to any SignalR hubs on the server, so let's avoid that
  // I don't wanna mock the whole connection, hence wrapper interface
  const mockClient: SignalRClient = {
    on: () => {},
    off: () => {},
    send: () => Promise.resolve()
  };
  this.set(SIGNALR_KEY, mockClient);
  return children;
}