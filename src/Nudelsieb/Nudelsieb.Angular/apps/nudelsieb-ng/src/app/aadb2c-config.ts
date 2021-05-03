/**
 * Enter here the user flows and custom policies for your B2C application,
 * To learn more about user flows, visit https://docs.microsoft.com/en-us/azure/active-directory-b2c/user-flow-overview
 * To learn more about custom policies, visit https://docs.microsoft.com/en-us/azure/active-directory-b2c/custom-policy-overview
 */
const tenantName = 'dzoech';
const aadTenant = `${tenantName}.onmicrosoft.com`;
const authorityDomain = `${tenantName}.b2clogin.com`;
const names = {
  policySignUpSignIn: 'B2C_1_email-user-flow',
  policyPasswordReset: 'B2C_1_passwort-reset-flow',
};

export const aadb2cPolicies = {
  names,
  authorities: {
    signUpSignIn: {
      authority: `https://${authorityDomain}/${aadTenant}/${names.policySignUpSignIn}`,
    },
    forgotPassword: {
      authority: `https://${authorityDomain}/${aadTenant}/${names.policyPasswordReset}`,
    },
  },
  aadTenant,
  authorityDomain,
};

/**
 * Enter here the coordinates of your Web API and scopes for access token request
 * The current application coordinates were pre-registered in a B2C tenant.
 */
export const apiConfig: { scopes: string[]; uri: string } = {
  scopes: [
    `https://${aadTenant}/nudelsieb/braindump.read`,
    `https://${aadTenant}/nudelsieb/braindump.write`,
  ],
  uri: 'https://localhost:5001/braindump/neuron',
};
